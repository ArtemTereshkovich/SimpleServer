using System;
using System.Net.Sockets;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using Polly;
using Polly.Timeout;
using Polly.Wrap;

namespace NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler.Decorators
{
    class ClientTransportHandlerWithRetry : IClientTransportHandler
    {
        private readonly IClientTransportHandler _origin;
        private readonly ClientTransportHandlerRetrySettings _retrySettings;
        private readonly ILogger _logger;

        public ClientTransportHandlerWithRetry(
            IClientTransportHandler origin,
            ClientTransportHandlerRetrySettings retrySettings,
            ILogger logger)
        {
            _origin = origin;
            _retrySettings = retrySettings;
            _logger = logger;
        }

        public void Dispose()
        {
            _origin.Dispose();
        }

        public void Activate(TransportHandlerContext context)
        {
            _origin.Activate(context);
        }

        public Packet AcceptedSend(Packet packet)
        {
            var retryPolicy = CreatePolicy();

            Packet receivedPacket = null;

            retryPolicy.Execute(() => { receivedPacket = _origin.AcceptedSend(packet); });

            if(receivedPacket == null)
                throw new ArgumentNullException(nameof(receivedPacket));

            return receivedPacket;
        }

        public void ClearReceiveBuffer()
        {
            _origin.ClearReceiveBuffer();
        }

        public void Close()
        {
            _origin.Close();
        }

        public void Reconnect()
        {
            _origin.Reconnect();
        }

        private PolicyWrap CreatePolicy()
        {
            var timeoutPolicy = Policy
                .Timeout(
                    _retrySettings.TimeOutAnswer,
                    TimeoutStrategy.Pessimistic,
                    (x, y, z) => TimeoutHandler());
;

            var retryPolicy =  Policy
                .Handle<SocketException>()
                .Or<ExecutionRejectedException>()
                .Or<SocketException>()
                .Or<AggregateException>()
                .Or<UnacceptedPacketException>()
                .WaitAndRetry(
                    _retrySettings.CountReconnect,
                    (_) => _retrySettings.ReconnectPeriod,
                    (exception, x, y) => RetryHandler(exception));

            return retryPolicy.Wrap(timeoutPolicy);
        }

        private void TimeoutHandler( )
        {
            _logger.LogError($"Timeout happend at send packet. Trying reconnect and repeat again....");
            _origin.Reconnect();
        }
        
        private void RetryHandler(Exception exception)
        {
            _logger.LogError($"Error happend at send packet. Exception:{exception.Message}. Trying reconnect and repeat again....");
            _origin.Reconnect();
        }

    }
}
