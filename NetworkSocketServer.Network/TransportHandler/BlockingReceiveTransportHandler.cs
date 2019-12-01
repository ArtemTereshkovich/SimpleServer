using System;
using System.Net.Sockets;

namespace NetworkSocketServer.NetworkLayer.TransportHandler
{
    internal class BlockingReceiveTransportHandler : ITransportHandler
    {
        private Socket _socket;

        public void Activate(Socket socket)
        {
            _socket = socket;
        }

        public void Send(byte[] array)
        {
            if(array == null || array.Length == 0)
                throw new ArgumentException(nameof(array));

            if(_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            _socket.Send(array);
        }

        public byte[] Receive()
        {
            if (_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            var checkBuffer = new byte[0];
            _socket.Receive(checkBuffer);

            System.Threading.Thread.Sleep(50);

            var buffer = new byte[_socket.Available];
            _socket.Receive(buffer);

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
            if(_socket.Connected)
                _socket.Close();

            _socket?.Dispose();
        }
    }
}
