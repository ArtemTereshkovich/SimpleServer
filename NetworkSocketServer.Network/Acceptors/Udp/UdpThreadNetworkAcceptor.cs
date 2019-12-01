using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.Acceptors.Udp
{
    internal class UdpThreadNetworkAcceptor : INetworkAcceptor
    {
        private readonly UdpNetworkAcceptorSettings _acceptorSettings;
        private readonly Socket _socket;
        private readonly object _lockObject;

        public UdpThreadNetworkAcceptor(UdpNetworkAcceptorSettings acceptorSettings)
        {
            _acceptorSettings = acceptorSettings;
            _socket = new Socket(
                _acceptorSettings.ListenIpAddress.AddressFamily,
                SocketType.Dgram, 
                ProtocolType.Udp); ;

            _lockObject = new object();
        }

        public void Open()
        {
            _socket.Bind(new IPEndPoint(_acceptorSettings.ListenIpAddress, _acceptorSettings.ListenPort));
        }

        public bool IsHaveNewConnection()
        {
            return _socket.Available != 0;
        }

        public Task AcceptConnection(ITransportHandler transportHandler)
        {
            transportHandler.Activate(_socket);

            return Task.CompletedTask;
        }

        public void Close()
        {
            _socket.Close();
        }
    }
}
