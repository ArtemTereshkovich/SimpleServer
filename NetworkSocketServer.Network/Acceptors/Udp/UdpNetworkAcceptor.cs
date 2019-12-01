using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.NetworkLayer.TransportHandler.NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.Acceptors.Udp
{
    internal class UdpNetworkAcceptor : INetworkAcceptor
    {
        private readonly UdpNetworkAcceptorSettings _acceptorSettings;
        private readonly Socket _socket;

        public UdpNetworkAcceptor(UdpNetworkAcceptorSettings acceptorSettings)
        {
            _acceptorSettings = acceptorSettings;

            _socket = new Socket(
                _acceptorSettings.ListenIpAddress.AddressFamily,
                SocketType.Dgram, 
                ProtocolType.Udp);
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
            var udpTransportHandler = transportHandler as UDPBlockingReceiveTransportHandler;

            udpTransportHandler.IpEndPointClient = ReceiveAddress();
                
            transportHandler.Activate(_socket);

            return Task.CompletedTask;
        }

        public void Close()
        {
            _socket.Close();
        }

        private IPEndPoint ReceiveAddress()
        {
            EndPoint ipEndPoint = null;
            var addressBytes = new byte[_socket.Available];
            _socket.ReceiveFrom(addressBytes, ref ipEndPoint);

            return IPEndPoint.Parse(Encoding.ASCII.GetString(addressBytes));
        }
    }
}
