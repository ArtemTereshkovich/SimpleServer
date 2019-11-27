using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor;
using NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.Connectors
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

        public Task Activate(ITransportHandler transportHandler)
        {
            var socket = new Socket(
                _networkConnectorSettings.IpEndPointServer.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            var accessor = _socketOptionsAccessorFactory.GetSocketOptionsAccessor();

            accessor.SetOptions(socket);

            socket.Connect(_networkConnectorSettings.IpEndPointServer);

            transportHandler.Activate(socket);

            return Task.CompletedTask;
        }
    }
}
