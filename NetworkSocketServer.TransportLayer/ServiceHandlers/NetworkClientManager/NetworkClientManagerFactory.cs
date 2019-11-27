using NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;
using NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.NetworkClientManager
{
    public class NetworkClientManagerFactory : INetworkClientManagerFactory
    {
        private readonly IConnectorDispatcherFactory _connectorDispatcherFactory;
        private readonly IRequestExecutorFactory _requestExecutorFactory;

        public NetworkClientManagerFactory(
            IConnectorDispatcherFactory connectorDispatcherFactory,
            IRequestExecutorFactory requestExecutorFactory)
        {
            _connectorDispatcherFactory = connectorDispatcherFactory;
            _requestExecutorFactory = requestExecutorFactory;
        }

        public INetworkClientManager Create(SocketKeepAliveOptions socketKeepAliveOptions)
        {
            var dispatcher = _connectorDispatcherFactory.CreateConnectorDispatcher(socketKeepAliveOptions);

            return new NetworkClientManager(dispatcher, _requestExecutorFactory);
        }
    }
}
