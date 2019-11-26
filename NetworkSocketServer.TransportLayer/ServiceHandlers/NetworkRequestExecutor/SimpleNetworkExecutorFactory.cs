using System;
using NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.NetworkRequestExecutor
{
    public class SimpleNetworkExecutorFactory : INetworkRequestExecutorFactory
    {
        private readonly IConnectorDispatcherFactory _connectorDispatcherFactory;

        public SimpleNetworkExecutorFactory(IConnectorDispatcherFactory connectorDispatcherFactory)
        {
            _connectorDispatcherFactory = connectorDispatcherFactory;
        }

        public INetworkRequestExecutor CreateExecutor(SocketKeepAliveOptions socketKeepAliveOptions)
        {
            var dispatcher = _connectorDispatcherFactory.CreateConnectorDispatcher(socketKeepAliveOptions);

            return new NetworkRequestExecutor(dispatcher);
        }
    }
}
