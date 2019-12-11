using System.Linq;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.Client.Logger;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler
{
    internal class WriteCommandHandler : SenderCommandHandler, INetworkCommandHandler
    {
        private readonly ServerSessionContext _serverSessionContext;
        private readonly IPacketFactory _packetFactory;

        public WriteCommandHandler(
            ServerSessionContext serverSessionContext,
            IPacketFactory packetFactory,
            ITransportHandler transportHandler, 
            IByteSerializer byteSerializer) 
            : base(transportHandler, byteSerializer)
        {
            _serverSessionContext = serverSessionContext;
            _packetFactory = packetFactory;
        }

        public Task<bool> Handle(Packet clientPacket)
        {
            CheckReceiveBuffer(clientPacket.BuffferSize);
            
            var payload = clientPacket.Payload.Take(clientPacket.PayloadSize).ToArray();
            
            new ConsoleClientLogger().LogProcessingBytes(clientPacket.BufferOffset, clientPacket.BuffferSize, clientPacket.PayloadSize);

            _serverSessionContext.ReceiveBuffer.Insert(payload, clientPacket.BufferOffset);

            var answerPacket = _packetFactory.CreateAnswerSuccessWrite(
                clientPacket.PacketId,
                _serverSessionContext.ReceiveBuffer.Length,
                clientPacket.BufferOffset,
                clientPacket.PayloadSize);

            SendPacket(answerPacket);

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
