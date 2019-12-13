namespace NetworkSocketServer.NetworkLayer.Server
{
    public interface IServiceConnectionManagerFactory
    {
        IServiceConnectionManager CreateConnectionManager();
    }
}
