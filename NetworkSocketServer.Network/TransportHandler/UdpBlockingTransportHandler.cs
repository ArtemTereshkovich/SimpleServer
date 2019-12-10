using System;
using System.Net;
using System.Net.Sockets;
using NetworkSocketServer.NetworkLayer.Connectors;


namespace NetworkSocketServer.NetworkLayer.TransportHandler
{
    public class UDPBlockingReceiveTransportHandler : ITransportHandler
    {
        private Socket _socket;

        public EndPoint IpEndPointClient;

        public bool EraseExceptionReceiveTimeout { get; set; } = false;

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

        public void ClearReceiveBuffer()
        {
            SafeClearBuffer();
        }

        public byte[] Receive()
        {
            if (_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            var waitingBuffer = new byte[0];
            _socket.ReceiveFrom(waitingBuffer, ref IpEndPointClient);

            var buffer = new byte[_socket.Available];
            _socket.ReceiveFrom(buffer, ref IpEndPointClient);

            return buffer;
        }

        public byte[] Receive(int length, bool eraseException)
        {
            WaitForData(length, eraseException);

            var buffer = new byte[length];
            _socket.Receive(buffer);

            return buffer;
        }
        public void Close()
        {
            SafeClearBuffer();

            if (_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            _socket.Close();
        }


        public void Reconnect(NetworkConnectorSettings connectorSettings)
        {
            SafeClearBuffer();

            _socket = new Socket(
                connectorSettings.IpEndPointServer.AddressFamily,
                SocketType.Dgram, ProtocolType.Udp);

            _socket.Connect(connectorSettings.IpEndPointServer);
        }

        public void Dispose()
        {
            SafeClearBuffer();
            _socket?.Dispose();
        }

        private void SafeClearBuffer()
        {
            try
            {
                var buffer = new byte[_socket.Available];
                _socket.ReceiveFrom(buffer, ref IpEndPointClient);
            }
            catch
            {

            }
        }

        private void WaitForData(int length, bool isErase)
        {
            if (isErase)
            {
                int counts = 200;
                while (counts != 0)
                {
                    if (_socket.Available >= length)
                        return;

                    counts--;

                    System.Threading.Thread.Sleep(10);
                }

                throw new InvalidOperationException("Receive UDP Timeout");
            }
            else
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(10);

                    if (_socket.Available >= length)
                        return;
                }
            }
        }

    }
}


