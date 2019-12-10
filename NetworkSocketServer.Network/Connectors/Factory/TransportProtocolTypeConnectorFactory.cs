using NetworkSocketServer.NetworkLayer.Connectors.Udp;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor;

namespace NetworkSocketServer.NetworkLayer.Connectors.Factory
{
    class TransportProtocolTypeConnectorFactory : INetworkConnectorFactory
    {
        private readonly ISocketOptionsAccessorFactory _socketOptionsAccessorFactory;

        public TransportProtocolTypeConnectorFactory(
            ISocketOptionsAccessorFactory socketOptionsAccessorFactory)
        {
            _socketOptionsAccessorFactory = socketOptionsAccessorFactory;
        }

        public INetworkConnector CreateNetworkConnector(NetworkConnectorSettings settings)
        {
            if (settings.ConnectionType == ConnectionType.Tcp)
            {
               return new TcpNetworkConnector(settings, _socketOptionsAccessorFactory);
            }
            else
            {

                return new UdpNetworkConnector(settings);
            }
        }
    }
}
