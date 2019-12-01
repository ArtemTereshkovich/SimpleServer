using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.NetworkLayer.TransportHandler.NetworkSocketServer.NetworkLayer.TransportHandler;

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

            SendLocalAddress(socket, _networkConnectorSettings.IpEndPointServer);

            var udpTransportHandler = transportHandler as UDPBlockingReceiveTransportHandler;

            udpTransportHandler.IpEndPointClient = _networkConnectorSettings.IpEndPointServer;

            transportHandler.Activate(socket);

            return Task.CompletedTask;
        }

        private void SendLocalAddress(Socket socket, EndPoint sendPoint)
        {
            var endPoint = socket.LocalEndPoint;

            socket.SendTo(Encoding.ASCII.GetBytes(endPoint.ToString()), sendPoint);


            System.Threading.Thread.Sleep(500);
        }
    }
}
