using NetworkSimpleServer.NetworkLayer.Client.Connectors.Factory;
using NetworkSimpleServer.NetworkLayer.Core.SocketOptionsAccessor.KeepAlive;

namespace NetworkSimpleServer.NetworkLayer.Client.ConnectorDispatcher
{
    public class SynchronousConnectorDispatcherFactory : IConnectorDispatcherFactory
    {
        public IConnectorDispatcher CreateConnectorDispatcher(SocketKeepAliveOptions options)
        {
            var socketOptionAccessor = new PlatformBasedKeepAliveAccessorFactory(options);

            var networkConnectorFactory = new TransportProtocolTypeConnectorFactory(socketOptionAccessor);

            return new SynchronousConnectorDispatcher(null, networkConnectorFactory);
        }
    }
}
