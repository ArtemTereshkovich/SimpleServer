using System.Net.Sockets;
using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;
using NetworkSimpleServer.NetworkLayer.Core.SocketOptionsAccessor;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Tcp;

namespace NetworkSimpleServer.NetworkLayer.Client.Connectors.Tcp
{
    internal class TcpNetworkConnector : INetworkConnector
    {
        private readonly NetworkConnectorSettings _networkConnectorSettings;
        private readonly ISocketOptionsAccessorFactory _socketOptionsAccessorFactory;

        public TcpNetworkConnector(
            NetworkConnectorSettings networkConnectorSettings,
            ISocketOptionsAccessorFactory socketOptionsAccessorFactory)
        {
            _networkConnectorSettings = networkConnectorSettings;
            _socketOptionsAccessorFactory = socketOptionsAccessorFactory;
        }

        public void Activate(IClientTransportHandler transportHandler)
        {
            var socket = new Socket(
                _networkConnectorSettings.IpEndPointServer.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

            var accessor = _socketOptionsAccessorFactory.GetSocketOptionsAccessor();

            accessor.SetOptions(socket);

            socket.Connect(_networkConnectorSettings.IpEndPointServer);

            transportHandler.Activate(new TcpTransportHandlerContext
            {
                AcceptedSocket = socket,
                RemoteEndPoint = _networkConnectorSettings.IpEndPointServer
            });
        }
    }
}
