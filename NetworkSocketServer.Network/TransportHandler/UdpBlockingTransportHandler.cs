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

        public bool EraseExceptionReceiveTimeout { get; set; }

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

            SendArrayLength(array);

            _socket.SendTo(array, IpEndPointClient);
        }

        private void SendArrayLength(byte[] array)
        {
            var arrayLength = array.Length;
            var bytesArrayLength = BitConverter.GetBytes(arrayLength);

            _socket.SendTo(bytesArrayLength, IpEndPointClient);
        }

        public byte[] Receive()
        {
            if (_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            int packetLength = ReceivePacketSize();

            return ReceivePacketSize(packetLength);
            
        }

        private byte[] ReceivePacketSize(int packetLength)
        {
            WaitForData(packetLength, EraseExceptionReceiveTimeout);
            var buffer = new byte[packetLength];
            _socket.ReceiveFrom(buffer, ref IpEndPointClient);

            return buffer;
        }

        public void WaitForData(int length, bool isErase)
        {
            if (isErase)
            {
                int counts = 200;
                while (counts != 0)
                {
                    if(_socket.Available >= length)
                        return;

                    counts--;

                    System.Threading.Thread.Sleep(10);
                }

                throw new InvalidOperationException("Receive UDP Timeout");
            }
            else
            {
                while (_socket.Available >= length)
                {
                    System.Threading.Thread.Sleep(10);
                }
            }
        }

        private int ReceivePacketSize()
        {
            while (_socket.Available <= 4)
            {
                System.Threading.Thread.Sleep(10);
            }

            var packetSizeBytes = new byte[4];

            _socket.ReceiveFrom(packetSizeBytes, ref IpEndPointClient);

            return BitConverter.ToInt32(packetSizeBytes);
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


