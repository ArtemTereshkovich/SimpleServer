using System.Net.Sockets;
using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Tcp;

namespace NetworkSimpleServer.NetworkLayer.Client.Connectors.Udp
{
    internal class UDPNetworkConnector : INetworkConnector
    {
        private readonly NetworkConnectorSettings _networkConnectorSettings;

        public UDPNetworkConnector(
            NetworkConnectorSettings networkConnectorSettings)
        {
            _networkConnectorSettings = networkConnectorSettings;
        }

        public void Activate(IClientTransportHandler transportHandler)
        {
            var socket = new Socket(
                _networkConnectorSettings.IpEndPointServer.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);
            
            socket.Connect(_networkConnectorSettings.IpEndPointServer);

            transportHandler.Activate(new TcpTransportHandlerContext
            {
                AcceptedSocket = socket,
                RemoteEndPoint = _networkConnectorSettings.IpEndPointServer
            });
        }
    }
}
