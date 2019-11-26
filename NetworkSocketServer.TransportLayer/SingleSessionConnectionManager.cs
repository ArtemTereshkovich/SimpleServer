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
    public class SingleSessionConnectionManager : IConnectionManager
    {
        private readonly IRequestHandlerFactory _requestHandlerFactory;
        private SessionContext _sessionContext;
        private readonly IByteSerializer _byteSerializer;
        private Guid _sessionId = Guid.Empty;

        public SingleSessionConnectionManager(IRequestHandlerFactory requestHandlerFactory)
        {
            _requestHandlerFactory = requestHandlerFactory;
            _byteSerializer = new BinaryFormatterByteSerializer();

        }

        public async Task HandleNewConnection(ITransportHandler transportHandler)
        {
            var packet = ReceivePacketMessage(transportHandler);

            CheckContext(packet);

            var packetHandler = CreatePacketHandler(
                transportHandler, _sessionContext, packet.SessionId);

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
            if (_sessionContext != null && _sessionId == packet.SessionId) return;

            _sessionContext = SessionContext.CreateNewMemoryStreamBufferContext();

            _sessionId = packet.SessionId;
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
