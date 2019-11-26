using System;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.Connectors;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.NetworkLayer.TransportHandler.Factories;

namespace NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher
{
    internal class ConnectorDispatcher : IConnectorDispatcher
    {
        private readonly ITransportHandlerFactory _transportHandlerFactory;
        private readonly ISocketOptionsAccessorFactory _socketOptionsAccessorFactory;

        public ConnectorDispatcher(
            ITransportHandlerFactory transportHandlerFactory,
            SocketKeepAliveOptions socketKeepAliveOptions)
        {
            _transportHandlerFactory = transportHandlerFactory;
            _socketOptionsAccessorFactory = 
                new PlatformBasedKeepAliveAccessorFactory(socketKeepAliveOptions);
        }

        public async Task<ITransportHandler> CreateTransportHandler(NetworkConnectorSettings settings)
        {
            var transportHandler = _transportHandlerFactory.CreateTransportHandler();

            if (settings.ConnectionType == ConnectionType.Tcp)
            {

                var connector = new TcpNetworkConnector(settings, _socketOptionsAccessorFactory);

                await connector.Activate(transportHandler);
            }
            else if (settings.ConnectionType == ConnectionType.Udp)
            {
                var connector = new UdpNetworkConnector(settings);

                await connector.Activate(transportHandler);
            }

            return transportHandler;
        }
    }
}
