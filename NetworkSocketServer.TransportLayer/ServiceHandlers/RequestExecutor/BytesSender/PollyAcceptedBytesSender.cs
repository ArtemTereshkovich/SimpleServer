using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor.BytesSender
{
    class PollyAcceptedBytesSender : IBytesSender
    {
        private readonly NetworkClientManager.NetworkClientManager _networkClientManager;
        private readonly RetrySettings _retrySettings;

        public PollyAcceptedBytesSender(
            NetworkClientManager.NetworkClientManager networkClientManager,
            RetrySettings retrySettings)
        {
            _networkClientManager = networkClientManager;
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

            await _networkClientManager.Reconnect();
        }

        private void Send(byte[] bytes)
        {
            _networkClientManager.SessionContext.TransportHandler.Send(bytes);
        }

        private byte[] Receive()
        {
            return _networkClientManager.SessionContext.TransportHandler.Receive();
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
                    _networkClientManager.SessionContext.TransportHandler.Send(sendBytes);
                }
                catch { }
            };

            var timeout =
                Policy.Timeout(settings.TimeOutAnswer, TimeoutStrategy.Pessimistic, (context, span, arg3) =>
                {
                    try { 
                        Console.WriteLine("Reconnection...");
                        Reconnect().Wait();
                        _networkClientManager.SessionContext.TransportHandler.Send(sendBytes);
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
