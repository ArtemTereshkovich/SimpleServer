using System.Net;

namespace NetworkSocketServer.NetworkLayer.TransportHandler
{
    using System;
    using System.Net.Sockets;

    namespace NetworkSocketServer.NetworkLayer.TransportHandler
    {
        internal class UDPBlockingReceiveTransportHandler : ITransportHandler
        {
            private Socket _socket;

            public EndPoint IpEndPointClient;

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

                var checkBuffer = new byte[0];
                _socket.ReceiveFrom(checkBuffer, ref IpEndPointClient);

                System.Threading.Thread.Sleep(100);

                var buffer = new byte[_socket.Available];
                _socket.ReceiveFrom(buffer, ref IpEndPointClient);

                return buffer;
            }

            public void Close()
            {
                if (_socket == null)
                    throw new InvalidOperationException(nameof(_socket));

                _socket.Close();
            }

            public void Dispose()
            {
                if (_socket.Connected)
                    _socket.Close();

                _socket?.Dispose();
            }
        }
    }

}
