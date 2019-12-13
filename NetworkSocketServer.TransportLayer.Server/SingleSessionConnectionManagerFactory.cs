using NetworkSocketServer.NetworkLayer.Server;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;

namespace NetworkSocketServer.TransportLayer.Server
{
    public class SingleSessionConnectionManagerFactory 
        : IServiceConnectionManagerFactory
    {
        private readonly IRequestHandlerFactory _factory;

        public SingleSessionConnectionManagerFactory(IRequestHandlerFactory factory)
        {
            _factory = factory;
        }

        public IServiceConnectionManager CreateConnectionManager()
        {
            return new SingleSessionConnectionManager(_factory);
        }
    }
}
