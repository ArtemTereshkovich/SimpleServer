using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Serializer;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base
{
    abstract class SenderCommandHandler
    {
        private readonly ITransportHandler _transportHandler;
        protected readonly IByteSerializer ByteSerializer;

        protected SenderCommandHandler(
            ITransportHandler transportHandler,
            IByteSerializer byteSerializer)
        {
            _transportHandler = transportHandler;
            ByteSerializer = byteSerializer;
        }

        protected void SendPacket(Packet packet)
        {
            var packetBytes = ByteSerializer.Serialize(packet);

            _transportHandler.Send(packetBytes);
        }
    }
}
