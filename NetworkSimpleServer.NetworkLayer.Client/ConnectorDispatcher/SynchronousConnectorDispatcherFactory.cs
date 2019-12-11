using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;
using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler.Factory;
using NetworkSimpleServer.NetworkLayer.Client.Connectors.Factory;
using NetworkSimpleServer.NetworkLayer.Core.SocketOptionsAccessor.KeepAlive;

namespace NetworkSimpleServer.NetworkLayer.Client.ConnectorDispatcher
{
    public class SynchronousConnectorDispatcherFactory : IConnectorDispatcherFactory
    {
        private readonly ClientTransportHandlerRetrySettings _retrySettings;

        public SynchronousConnectorDispatcherFactory(ClientTransportHandlerRetrySettings retrySettings)
        {
            _retrySettings = retrySettings;
        }

        public IConnectorDispatcher CreateConnectorDispatcher(SocketKeepAliveOptions options)
        {
            var socketOptionAccessor = new PlatformBasedKeepAliveAccessorFactory(options);

            var networkConnectorFactory = new TransportProtocolTypeConnectorFactory(socketOptionAccessor);

            return new SynchronousConnectorDispatcher(
                new AcceptedRetryClientTransportHandlerFactory(_retrySettings),
                networkConnectorFactory);
        }
    }
}
