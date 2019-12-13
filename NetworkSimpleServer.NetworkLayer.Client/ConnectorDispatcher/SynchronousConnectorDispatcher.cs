using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;
using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler.Factory;
using NetworkSimpleServer.NetworkLayer.Client.Connectors;
using NetworkSimpleServer.NetworkLayer.Client.Connectors.Factory;

namespace NetworkSimpleServer.NetworkLayer.Client.ConnectorDispatcher
{
    internal class SynchronousConnectorDispatcher : IConnectorDispatcher
    {
        private readonly IClientTransportHandlerFactory _clientTransportHandlerFactory;
        private readonly INetworkConnectorFactory _networkConnectorFactory;

        public SynchronousConnectorDispatcher(
            IClientTransportHandlerFactory clientTransportHandlerFactory,
            INetworkConnectorFactory networkConnectorFactory)
        {
            _clientTransportHandlerFactory = clientTransportHandlerFactory;
            _networkConnectorFactory = networkConnectorFactory;
        }

        public IClientTransportHandler CreateClientTransportHandler(NetworkConnectorSettings settings)
        {
            var transportHandler = _clientTransportHandlerFactory.CreateTransportHandler();

            var connector = _networkConnectorFactory.CreateNetworkConnector(settings);

            connector.Activate(transportHandler);

            return transportHandler;
        }
    }
}
