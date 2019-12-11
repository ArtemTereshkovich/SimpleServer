using NetworkSocketServer.TransportLayer.Server.IRequestHandler;

namespace NetworkSocketServer.Server
{
    class SimpleRequestHandlerFactory : IRequestHandlerFactory
    {
        public IRequestHandler CreateRequestHandler()
        {
            return new SimpleRequestHandler();
        }
    }
}
