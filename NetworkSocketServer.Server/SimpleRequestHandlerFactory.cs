using NetworkSocketServer.TransportLayer.Server.IRequestHandler;
using NetworkSocketServer.TransportLayer.ServiceHandlers;

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
