using NetworkSocketServer.TransportLayer.Server.IRequestHandler;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers
{
    public interface IRequestHandlerFactory
    {
        IRequestHandler CreateRequestHandler();
    }
}
