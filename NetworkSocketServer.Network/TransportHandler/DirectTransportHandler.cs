using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkSocketServer.Network.TransportHandler
{
    internal class DirectTransportHandler : ITransportHandler
    {
        private Socket _socket;

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

            var zeroBuffer = new byte[0];
            _socket.Receive(zeroBuffer);

            var buffer = new byte[_socket.Available];
            _socket.Receive(buffer);

            return Task.FromResult(buffer);
        }

        public void Close()
        {
            _socket.Close();
        }
    }
}
