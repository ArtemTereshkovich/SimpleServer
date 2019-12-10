using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.Acceptors.Udp
{
    public class UdpNetworkAcceptor : INetworkAcceptor
    {
        private readonly UdpNetworkAcceptorSettings _acceptorSettings;
        private Socket _socket;

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
            if (transportHandler is UDPBlockingReceiveTransportHandler udpTransportHandler)
            {
                udpTransportHandler.IpEndPointClient = ReceiveAddress();
                udpTransportHandler.EraseExceptionReceiveTimeout = true;
            }
            
            transportHandler.Activate(_socket);

            return Task.CompletedTask;
        }

        public void Close()
        {
            _socket.Close();
        }

        public void Release()
        {
            _socket = new Socket(
                _acceptorSettings.ListenIpAddress.AddressFamily,
                SocketType.Dgram,
                ProtocolType.Udp);

            _socket.Bind(new IPEndPoint(_acceptorSettings.ListenIpAddress, _acceptorSettings.ListenPort));
        }

        private IPEndPoint ReceiveAddress()
        {
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint senderRemote = sender;
            var addressBytes = new byte[_socket.Available];
            _socket.ReceiveFrom(addressBytes, ref senderRemote);

            return IPEndPoint.Parse(Encoding.ASCII.GetString(addressBytes));
        }
    }
}
