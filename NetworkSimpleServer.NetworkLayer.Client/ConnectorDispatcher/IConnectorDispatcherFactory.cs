using NetworkSimpleServer.NetworkLayer.Core.SocketOptionsAccessor.KeepAlive;

namespace NetworkSimpleServer.NetworkLayer.Client.ConnectorDispatcher
{
    public interface IConnectorDispatcherFactory
    {
        IConnectorDispatcher CreateConnectorDispatcher(SocketKeepAliveOptions options);
    }
}
