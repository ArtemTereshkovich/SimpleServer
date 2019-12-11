using System;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler
{
    internal class ReadCommandHandler : INetworkCommandHandler
    {
        private readonly ILogger _logger;
        private readonly ServerSessionContext _serverSessionContext;
        private readonly IPacketFactory _packetFactory;
        private readonly ITransportHandler _transportHandler;

        public ReadCommandHandler(
            ILogger logger,
            ServerSessionContext serverSessionContext,
            IPacketFactory packetFactory,
            ITransportHandler transportHandler) 
        {
            _logger = logger;
            _serverSessionContext = serverSessionContext;
            _packetFactory = packetFactory;
            _transportHandler = transportHandler;
        }

        public Task<bool> Handle(Packet clientPacket)
        {
            CheckTransmitBuffer();

            _logger.LogProcessingBytes(
                clientPacket.BufferOffset, 
                _serverSessionContext.TransmitBuffer.Length,
                clientPacket.PayloadSize);

            var array = _serverSessionContext.TransmitBuffer.Get(clientPacket.BufferOffset, clientPacket.PayloadSize);

            var arrayLength = array.Length;

            if (arrayLength < PacketConstants.PacketPayloadThresholdSize)
            {
                Array.Resize(ref array, PacketConstants.PacketPayloadThresholdSize);
            }

            var answerPacket = _packetFactory.CreateAnswerSuccessRead(
                clientPacket.PacketId,
                array,
                _serverSessionContext.TransmitBuffer.Length,
                clientPacket.BufferOffset,
                arrayLength);

            _transportHandler.Send(answerPacket);

            return Task.FromResult(true);
        }

        private void CheckTransmitBuffer()
        {
            if (_serverSessionContext.TransmitBuffer.Length == 0)
            {
                throw new InvalidOperationException(
                    nameof(_serverSessionContext.TransmitBuffer) + " is Empty.");
            }
        }
    }
}
