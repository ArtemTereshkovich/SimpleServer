using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkSocketServer.NetworkLayer.TransportHandler
{
    internal class BlockingReceiveTransportHandler : ITransportHandler
    {
        private Socket _socket;

        private int _receiveFragmentSize = 1024;

        public int ReceiveFragmentSize
        {
            get => _receiveFragmentSize;
            set
            {
                if(value <= 0)
                    throw new ArgumentException(nameof(value));

                _receiveFragmentSize = value;
            }
        }

        public void Activate(Socket socket)
        {
            _socket = socket;
        }

        public Task Send(byte[] array)
        {
            if(array == null || array.Length == 0)
                throw new ArgumentException(nameof(array));

            if(_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            _socket.Send(array);

            return Task.CompletedTask;
        }

        public Task<byte[]> Receive()
        {
            if (_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            var checkBuffer = new byte[0];
            _socket.Receive(checkBuffer);

            var buffer = new byte[GetReceiveSegmentSize()];
            _socket.Receive(buffer);
            
            return Task.FromResult(buffer);
        }

        public Task<byte[]> ReceiveAllAvailable()
        {
            if (_socket == null)
                throw new InvalidOperationException(nameof(_socket));

            var checkBuffer = new byte[0];
            _socket.Receive(checkBuffer);

            var buffer = new byte[_socket.Available];
            _socket.Receive(buffer);

            return Task.FromResult(buffer);
        }

        private int GetReceiveSegmentSize()
        {
            return Math.Min(_socket.Available, ReceiveFragmentSize);
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
