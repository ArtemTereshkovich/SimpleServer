using NetworkSimpleServer.NetworkLayer.Client.ConnectorDispatcher;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.SocketOptionsAccessor.KeepAlive;
using NetworkSocketServer.TransportLayer.Client.ClientManager;
using NetworkSocketServer.TransportLayer.Client.TransportManager;

namespace NetworkSocketServer.TransportLayer.Client.ConnectionManager
{
    public class ClientConnectionManagerFactory : IClientConnectionManagerFactory
    {
        private readonly IConnectorDispatcherFactory _connectorDispatcherFactory;
        private readonly IClientTransportManagerFactory _clientTransportManagerFactory;

        public ClientConnectionManagerFactory(
            IConnectorDispatcherFactory connectorDispatcherFactory,
            IClientTransportManagerFactory clientTransportManagerFactory)
        {
            _connectorDispatcherFactory = connectorDispatcherFactory;
            _clientTransportManagerFactory = clientTransportManagerFactory;
        }

        public IClientConnectionManager Create(SocketKeepAliveOptions socketKeepAliveOptions)
        {
            var dispatcher = _connectorDispatcherFactory.CreateConnectorDispatcher(socketKeepAliveOptions);

            return new ClientConnectionManager(
                dispatcher, 
                _clientTransportManagerFactory, 
                new ConsoleLogger());
        }
    }
}
