using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using NetworkSocketServer.TransportLayer.Client.ConnectionManager;
using NetworkSocketServer.TransportLayer.Client.RequestExecutor;
using NetworkSocketServer.TransportLayer.Client.ServiceHandlers.RequestExecutor.BytesSender;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager.BytesSender
{
    class PollyAcceptedBytesSender : IBytesSender
    {
        private readonly ClientConnectionManager _clientConnectionManager;
        private readonly RetrySettings _retrySettings;

        public PollyAcceptedBytesSender(
            ClientConnectionManager clientConnectionManager,
            RetrySettings retrySettings)
        {
            _clientConnectionManager = clientConnectionManager;
            _retrySettings = retrySettings;
        }

        public async Task<byte[]> AcceptedSend(byte[] bytes)
        {
            var sendRetry = CreateSendPolicy(_retrySettings);

            var receivePolicy = CreateReceivePolicy(_retrySettings, bytes);

            await sendRetry.ExecuteAsync(() =>
            {
                Send(bytes);
                return Task.CompletedTask;
            });

            byte[] receiveBytes = null;

            receivePolicy.Execute(() => { receiveBytes = Receive(); });

            return receiveBytes;
        }

        private async Task Reconnect()
        {
            Console.WriteLine($"Trying reconnect.");

            await _clientConnectionManager.Reconnect();
        }

        private void Send(byte[] bytes)
        {
            _clientConnectionManager.SessionContext.TransportHandler.Send(bytes);
        }

        private byte[] Receive()
        {
            return _clientConnectionManager.SessionContext.TransportHandler.Receive();
        }

        private AsyncRetryPolicy CreateSendPolicy(RetrySettings settings)
        {
            Func<Exception, TimeSpan, Task> onRetryAsync = async (exc, time) =>
            {
                Console.WriteLine("Reconnection...");
                await Reconnect();
            };

            return
                Policy.Handle<SocketException>()
                    .WaitAndRetryAsync(
                        settings.CountReconnect,
                        (_) => settings.ReconnectPeriod,
                        onRetryAsync);
        }

        private PolicyWrap CreateReceivePolicy(RetrySettings settings, byte[] sendBytes)
        {
            
            Action<Exception, TimeSpan> onRetryAsync = (exc, time) =>
            {
                try
                {
                    Console.WriteLine("Reconnection...");
                    Reconnect().Wait();
                    _clientConnectionManager.SessionContext.TransportHandler.Send(sendBytes);
                }
                catch { }
            };

            var timeout =
                Policy.Timeout(settings.TimeOutAnswer, TimeoutStrategy.Pessimistic, (context, span, arg3) =>
                {
                    try { 
                        Console.WriteLine("Reconnection...");
                        Reconnect().Wait();
                        _clientConnectionManager.SessionContext.TransportHandler.Send(sendBytes);
                    }catch { }
                });

            var retry =
                Policy
                    .Handle<Exception>()
                    .Or<ExecutionRejectedException>()
                    .Or<SocketException>()
                    .Or<AggregateException>()
                    .WaitAndRetry(
                        settings.CountReconnect,
                        (_) => settings.ReconnectPeriod,
                        onRetryAsync);

            return retry.Wrap(timeout);
        }
    }
}
