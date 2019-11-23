using System.Net.Sockets;
using System.Threading.Tasks;
using Polly;

namespace NetworkSocketServer.Network.TransportHandler
{
    internal class TransportHandlerWithRetry : ITransportHandler
    {
        private readonly TransportHandlerRetryOptions _options;
        private readonly ITransportHandler _origin;

        public TransportHandlerWithRetry(TransportHandlerRetryOptions options, ITransportHandler origin)
        {
            _options = options;
            _origin = origin;
        }

        public void Activate(Socket socket)
        {
            _origin.Activate(socket);
        }

        public async Task Send(byte[] array)
        {
            var retryPolicy = Policy
              .Handle<SocketException>(
                  exception => exception.SocketErrorCode == SocketError.NetworkDown)
              .WaitAndRetryAsync(
                  _options.RetryCount,
                  retryAttempt => _options.RetryInterval);

            await retryPolicy.ExecuteAsync(async () => await _origin.Send(array));

        }

        public async Task<byte[]> Receive()
        {
            var retryPolicy = Policy
                .Handle<SocketException>(
                    exception => exception.SocketErrorCode == SocketError.NetworkDown)
                .WaitAndRetryAsync(
                    _options.RetryCount, 
                    retryAttempt => _options.RetryInterval);

            return await retryPolicy.ExecuteAsync(async () => await _origin.Receive());
        }

        public void Close()
        {
            _origin.Close();
        }
    }
}
