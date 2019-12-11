using System.Linq;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler
{
    internal class WriteCommandHandler : INetworkCommandHandler
    {
        private readonly ServerSessionContext _serverSessionContext;
        private readonly IPacketFactory _packetFactory;
        private readonly ITransportHandler _transportHandler;
        private readonly ILogger _logger;

        public WriteCommandHandler(
            ServerSessionContext serverSessionContext,
            IPacketFactory packetFactory,
            ITransportHandler transportHandler,
            ILogger logger) 
        {
            _serverSessionContext = serverSessionContext;
            _packetFactory = packetFactory;
            _transportHandler = transportHandler;
            _logger = logger;
        }

        public Task<bool> Handle(Packet clientPacket)
        {
            CheckReceiveBuffer(clientPacket.BuffferSize);
            
            var payload = clientPacket.Payload.Take(clientPacket.PayloadSize).ToArray();
            
            _logger.LogProcessingBytes(clientPacket.BufferOffset, clientPacket.BuffferSize, clientPacket.PayloadSize);

            _serverSessionContext.ReceiveBuffer.Insert(payload, clientPacket.BufferOffset);

            var answerPacket = _packetFactory.CreateAnswerSuccessWrite(
                clientPacket.PacketId,
                _serverSessionContext.ReceiveBuffer.Length,
                clientPacket.BufferOffset,
                clientPacket.PayloadSize);

            _transportHandler.Send(answerPacket);

            return Task.FromResult(true);
        }

        private void CheckReceiveBuffer(int payloadSize)
        {
            if (_serverSessionContext.ReceiveBuffer.Length != payloadSize)
            {
                UpdateReceiveBuffer(payloadSize);
            }
        }

        private void UpdateReceiveBuffer(int contentLength)
        {
            _serverSessionContext.TransmitBuffer.Reinitialize(contentLength);
        }
    }
}
