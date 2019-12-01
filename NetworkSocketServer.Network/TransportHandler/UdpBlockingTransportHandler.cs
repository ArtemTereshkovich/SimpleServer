using System;
using System.Net;
using System.Net.Sockets;
using NetworkSocketServer.NetworkLayer.Acceptors.Udp;
using NetworkSocketServer.NetworkLayer.Connectors;


namespace NetworkSocketServer.NetworkLayer.TransportHandler
{
    public class UDPBlockingReceiveTransportHandler : ITransportHandler
    {
        private Socket _socket;

        public EndPoint IpEndPointClient;

        public UdpNetworkAcceptor UdpNetworkAcceptor;

        public void Activate(Socket socket)
        {
            _socket = socket;
        }

        public void Send(byte[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException(nameof(array));

            if (_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            _socket.SendTo(array, IpEndPointClient);
        }

        public byte[] Receive()
        {
            if (_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            WaitForPacket();

            var buffer = new byte[_socket.Available];
            _socket.ReceiveFrom(buffer, ref IpEndPointClient);

            return buffer;
        }

        private void WaitForPacket()
        {
            while (_socket.Available == 0)
            {
                System.Threading.Thread.Sleep(100);
            }

            System.Threading.Thread.Sleep(400);
        }

        public void Close()
        {
            UdpNetworkAcceptor?.Release();

            if (_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            _socket.Close();
        }

        public void Dispose()
        {
            UdpNetworkAcceptor?.Release();
        }

        public void Reconnect(NetworkConnectorSettings connectorSettings)
        {
            _socket = new Socket(
                connectorSettings.IpEndPointServer.AddressFamily,
                SocketType.Dgram, ProtocolType.Udp);

            _socket.Connect(connectorSettings.IpEndPointServer);
        }
    }
}


