using System.Net;
using System.Net.Sockets;
using System.Text;
using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Udp;

namespace NetworkSimpleServer.NetworkLayer.Client.Connectors.Udp
{
    internal class UdpNetworkConnector : INetworkConnector
    {
        private readonly NetworkConnectorSettings _networkConnectorSettings;

        public UdpNetworkConnector(NetworkConnectorSettings networkConnectorSettings)
        {
            _networkConnectorSettings = networkConnectorSettings;
        }

        public void Activate(IClientTransportHandler transportHandler)
        {
            var socket = new Socket(
                _networkConnectorSettings.IpEndPointServer.AddressFamily,
                SocketType.Dgram, ProtocolType.Udp);

            socket.Connect(_networkConnectorSettings.IpEndPointServer);

            SendLocalAddress(socket, _networkConnectorSettings.IpEndPointServer);

            transportHandler.Activate(new UdpTransportHandlerContext
            {
                AcceptedSocket = socket,
                RemoteEndPoint = _networkConnectorSettings.IpEndPointServer
            });
        }

        private static void SendLocalAddress(Socket socket, EndPoint sendPoint)
        {
            var endPoint = socket.LocalEndPoint;

            socket.SendTo(Encoding.ASCII.GetBytes(endPoint.ToString()), sendPoint);

            System.Threading.Thread.Sleep(500);
        }
    }
}
