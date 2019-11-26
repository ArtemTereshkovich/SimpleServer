using System.Net.Sockets;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.Connectors
{
    internal class UdpNetworkConnector : INetworkConnector
    {
        private readonly NetworkConnectorSettings _networkConnectorSettings;

        public UdpNetworkConnector(NetworkConnectorSettings networkConnectorSettings)
        {
            _networkConnectorSettings = networkConnectorSettings;
        }

        public Task Activate(ITransportHandler transportHandler)
        {
            var socket = new Socket(
                _networkConnectorSettings.IpEndPointServer.AddressFamily,
                SocketType.Dgram, ProtocolType.Udp);

            socket.Connect(_networkConnectorSettings.IpEndPointServer);

            transportHandler.Activate(socket);

            return Task.CompletedTask;
        }
    }
}
