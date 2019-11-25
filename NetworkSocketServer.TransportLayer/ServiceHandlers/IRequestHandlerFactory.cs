namespace NetworkSocketServer.TransportLayer.ServiceHandlers
{
    public interface IRequestHandlerFactory
    {
        IRequestHandler CreateRequestHandler();
    }
}
