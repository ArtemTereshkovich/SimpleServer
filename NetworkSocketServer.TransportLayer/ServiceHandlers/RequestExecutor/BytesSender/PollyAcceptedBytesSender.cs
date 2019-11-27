using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor.BytesSender
{
    class PollyAcceptedBytesSender : IBytesSender
    {
        private readonly NetworkClientManager.NetworkClientManager _networkClientManager;
        private readonly RetrySettings _retrySettings;
        private readonly Policy _receivePolicy;
        private readonly Policy _sendPolicy;

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

            var (receiveTimeout, receiveRetry) = CreateReceivePolicy(_retrySettings, bytes);

            await sendRetry.ExecuteAsync(() =>
            {
                Send(bytes);
                return Task.CompletedTask;
            });

            byte[] receiveBytes = null;

            var receiveTask = receiveRetry.ExecuteAsync(() =>
            {
                receiveBytes = Receive();
                return Task.CompletedTask;
            });

            await receiveRetry.ExecuteAsync(async () => { await receiveTask; }
            );

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
            Func<Exception, TimeSpan, Task> onRetryAsync = async (exc, time) => { await Reconnect(); };

            return
                Policy.Handle<SocketException>()
                    .WaitAndRetryAsync(
                        settings.CountReconnect,
                        (_) => settings.ReconnectPeriod,
                        onRetryAsync);
        }

        private (TimeoutPolicy timeoutPolicy, AsyncRetryPolicy asyncRetryPolicy) CreateReceivePolicy(RetrySettings settings, byte[] sendBytes)
        {
            
            Func<Exception, TimeSpan, Task> onRetryAsync = async (exc, time) =>
            {
                await Reconnect();
                _networkClientManager.SessionContext.TransportHandler.Send(sendBytes);
            };

            var timeout =
                Policy.Timeout(settings.TimeOutAnswer,
                    (a, b, c) =>
                    {
                        Console.WriteLine("Timeout Receive Answer Packet");
                    });

            var retry =
                Policy.Handle<SocketException>()
                    .Or<TimeoutRejectedException>()
                    .WaitAndRetryAsync(
                        settings.CountReconnect,
                        (_) => settings.ReconnectPeriod,
                        onRetryAsync);

            return (timeout, retry);
        }
    }
}
