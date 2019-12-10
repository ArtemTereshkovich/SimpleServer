using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Context;

namespace NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler.Decorators
{
    class ClientTransportHandlerWithRetry : IClientTransportHandler
    {
        private readonly IClientTransportHandler _origin;
        private readonly ILogger _logger;

        public ClientTransportHandlerWithRetry(
            IClientTransportHandler origin,
            ClientTransportHandlerRetrySettings retrySettings,
            ILogger logger)
        {
            _origin = origin;
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
    }
}
