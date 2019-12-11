using NetworkSimpleServer.NetworkLayer.Client.Connectors.Tcp;
using NetworkSimpleServer.NetworkLayer.Client.Connectors.Udp;
using NetworkSimpleServer.NetworkLayer.Core.SocketOptionsAccessor;

namespace NetworkSimpleServer.NetworkLayer.Client.Connectors.Factory
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

                return new UDPNetworkConnector(settings);
            }
        }
    }
}
