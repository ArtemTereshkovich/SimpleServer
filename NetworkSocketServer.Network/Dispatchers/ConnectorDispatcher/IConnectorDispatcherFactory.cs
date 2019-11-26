using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;

namespace NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher
{
    public interface IConnectorDispatcherFactory
    {
        IConnectorDispatcher CreateConnectorDispatcher(SocketKeepAliveOptions options);
    }
}
