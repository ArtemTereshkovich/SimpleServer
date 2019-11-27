namespace NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor
{
    public interface IRequestExecutorFactory
    {
        IRequestExecutor Create(NetworkClientManager.NetworkClientManager networkClientManager);
    }
}
