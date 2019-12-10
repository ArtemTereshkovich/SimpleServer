using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.Connectors;
using NetworkSocketServer.NetworkLayer.Connectors.Factory;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.NetworkLayer.TransportHandler.Factories;

namespace NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher
{
    internal class ConnectorDispatcher : IConnectorDispatcher
    {
        private readonly ITransportHandlerFactory _transportHandlerFactory;
        private readonly INetworkConnectorFactory _networkConnectorFactory;

        public ConnectorDispatcher(
            ITransportHandlerFactory transportHandlerFactory,
            SocketKeepAliveOptions socketKeepAliveOptions)
        {
            _transportHandlerFactory = transportHandlerFactory;
            
            var socketOptionsAccessorFactory = 
                new PlatformBasedKeepAliveAccessorFactory(socketKeepAliveOptions);

            _networkConnectorFactory = new TransportProtocolTypeConnectorFactory(
                socketOptionsAccessorFactory);
        }

        public async Task<ITransportHandler> CreateTransportHandler(NetworkConnectorSettings settings)
        {
            var transportHandler = _transportHandlerFactory.CreateTransportHandler(settings.ConnectionType);

            var connector = _networkConnectorFactory.CreateNetworkConnector(settings);

            await connector.Activate(transportHandler);

            return transportHandler;
        }
    }
}
