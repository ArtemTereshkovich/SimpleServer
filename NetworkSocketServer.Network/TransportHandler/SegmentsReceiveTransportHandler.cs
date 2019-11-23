using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkSocketServer.Network.TransportHandler
{
    class SegmentsReceiveTransportHandler : ITransportHandler
    {
        private readonly int _receiveSize;
        private Socket _socket;

        public SegmentsReceiveTransportHandler(int receiveSize)
        {
            _receiveSize = receiveSize;
            throw new System.NotImplementedException();
        }

        public void Activate(Socket socket)
        {
            if (_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            _socket = socket;
        }

        public Task Send(byte[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException(nameof(array));

            if (_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            _socket.Send(array);

            return Task.CompletedTask;
        }

        public Task<byte[]> Receive()
        {
            if (_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            var buffer = new byte[_socket.Available];
            _socket.Receive(buffer);

            return Task.FromResult(buffer);
        }

        public void Close()
        {
            if (_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            _socket.Close();
        }
    }
}