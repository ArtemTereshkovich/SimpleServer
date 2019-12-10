using System;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.Client.Logger;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler
{
    internal class ReadCommandHandler : SenderCommandHandler, INetworkCommandHandler
    {
        private readonly ServerSessionContext _serverSessionContext;
        private readonly IPacketFactory _packetFactory;

        public ReadCommandHandler(
            ServerSessionContext serverSessionContext,
            IPacketFactory packetFactory,
            ITransportHandler transportHandler,
            IByteSerializer byteSerializer) 
            : base(transportHandler, byteSerializer)
        {
            _serverSessionContext = serverSessionContext;
            _packetFactory = packetFactory;
        }

        public Task<bool> Handle(Packet packet)
        {
            CheckTransmitBuffer();


            new ConsoleClientLogger().LogProcessingBytes(
                packet.BufferOffset, 
                _serverSessionContext.TransmitBuffer.Length,
                packet.PayloadSize);

            var array = _serverSessionContext.TransmitBuffer.Get(packet.BufferOffset, packet.PayloadSize);

            var answerPacket = _packetFactory.CreateAnswerSuccessRead(
                array,
                _serverSessionContext.TransmitBuffer.Length,
                packet.BufferOffset, 
                array.Length);

            SendPacket(answerPacket);

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
