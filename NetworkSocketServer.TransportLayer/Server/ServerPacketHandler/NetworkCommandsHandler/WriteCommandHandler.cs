using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.PacketHandler.NetworkCommandsHandler.Base;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.Server;

namespace NetworkSocketServer.TransportLayer.PacketHandler.NetworkCommandsHandler
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

        public Task<bool> Handle(Packet packet)
        {
            CheckReceiveBuffer(packet.Size);

            _serverSessionContext.ReceiveBuffer.Insert(packet.Payload, packet.Offset);

            var answerPacket = _packetFactory.CreateAnswerSuccessWrite(
                _serverSessionContext.ReceiveBuffer.Length,
                packet.Payload.Length);

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
