namespace NetworkSocketServer.TransportLayer.Server.IRequestHandler
{
    public interface IRequestHandlerFactory
    {
        IRequestHandler CreateRequestHandler(int serviceId);
    }
}
