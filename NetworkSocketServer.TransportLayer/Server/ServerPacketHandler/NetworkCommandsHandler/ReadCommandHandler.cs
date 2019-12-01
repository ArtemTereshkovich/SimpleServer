using System;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.PacketHandler.NetworkCommandsHandler.Base;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.Server;

namespace NetworkSocketServer.TransportLayer.PacketHandler.NetworkCommandsHandler
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

            var array = _serverSessionContext.TransmitBuffer.Get(packet.Offset, packet.Size);

            var answerPacket = _packetFactory.CreateAnswerSuccessRead(
                array,
                _serverSessionContext.TransmitBuffer.Length,
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
