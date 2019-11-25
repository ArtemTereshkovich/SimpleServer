using System;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.DTO;
using NetworkSocketServer.TransportLayer.PacketHandler;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.ServiceHandlers;

namespace NetworkSocketServer.TransportLayer
{
    public class NewConnectionHandler : INewTransportHandler
    {
        private readonly IRequestHandlerFactory _requestHandlerFactory;
        private SessionContext _sessionContext;
        private readonly IByteSerializer _byteSerializer;
        private Guid _connectionId = Guid.Empty;

        public NewConnectionHandler(IRequestHandlerFactory requestHandlerFactory)
        {
            _requestHandlerFactory = requestHandlerFactory;
            _byteSerializer = new BinaryFormatterByteSerializer();

        }

        public async Task HandleNewConnection(ITransportHandler transportHandler)
        {
            var packet = ReceivePacketMessage(transportHandler);

            CheckContext(packet);

            var packetHandler = CreatePacketHandler(
                transportHandler, _sessionContext, packet.ConnectionId);

            await packetHandler.HandlePacket(packet);

            await ContinueProcessingPacket(packetHandler, transportHandler);
        }

        private async Task ContinueProcessingPacket(
            IPacketHandler packetHandler,ITransportHandler transportHandler)
        {
            while (true)
            {
                var packet = ReceivePacketMessage(transportHandler);

                await packetHandler.HandlePacket(packet);
            }
        }

        private void CheckContext(Packet packet)
        {
            if (_sessionContext != null && _connectionId == packet.ConnectionId) return;

            _sessionContext = SessionContext.CreateNewMemoryStreamBufferContext();

            _connectionId = packet.ConnectionId;
        }

        private Packet ReceivePacketMessage(ITransportHandler transportHandler)
        {
            var messageBytes = transportHandler.Receive();

            return _byteSerializer.Deserialize<Packet>(messageBytes);
        }

        private IPacketHandler CreatePacketHandler(
            ITransportHandler transportHandler,
            SessionContext sessionContext,
            Guid connectionId)
        {
            return new ServerPredictBasedPacketHandler(
                connectionId,
                _requestHandlerFactory,
                transportHandler,
                sessionContext);
        }
    }
}
