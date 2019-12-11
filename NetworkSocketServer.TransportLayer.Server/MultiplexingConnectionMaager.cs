using System;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSimpleServer.NetworkLayer.Server;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;
using NetworkSocketServer.TransportLayer.Core.Serializer;
using NetworkSocketServer.TransportLayer.PacketHandler;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler;

namespace NetworkSocketServer.TransportLayer.Server
{
    public class MultiplexingConnectionManager : IServiceConnectionManager
    {
        private readonly IRequestHandlerFactory _requestHandlerFactory;

        public MultiplexingConnectionManager(IRequestHandlerFactory requestHandlerFactory)
        {
            _requestHandlerFactory = requestHandlerFactory;
        }

        public Task HandleNewConnection(ITransportHandler transportHandler)
        {
            throw new NotImplementedException();
        }

        public Task ProcessRegistered()
        {
            throw new NotImplementedException();
        }

        private IServerPacketHandler CreatePacketHandler(
            ITransportHandler transportHandler,
            ServerSessionContext serverSessionContext)
        {
            return new ServerPredictBasedServerPacketHandler(
                _requestHandlerFactory,
                transportHandler,
                new PacketFactory(serverSessionContext.SessionId),
                new BinaryFormatterBytesSerializer(),
                new ConsoleLogger(),
                serverSessionContext);
        }
    }
}
