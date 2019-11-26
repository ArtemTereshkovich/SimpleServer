using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;
using NetworkSocketServer.NetworkLayer.TransportHandler.Factories;

namespace NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher
{
    public class ConnectorDispatcherFactory : IConnectorDispatcherFactory
    {
        public IConnectorDispatcher CreateConnectorDispatcher(SocketKeepAliveOptions options)
        {
            return new ConnectorDispatcher(new BlockingTransportHandlerFactory(), options);
        }
    }
}
