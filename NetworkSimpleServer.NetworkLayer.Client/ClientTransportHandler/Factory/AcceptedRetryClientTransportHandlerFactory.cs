using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler.Decorators;
using NetworkSimpleServer.NetworkLayer.Core;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.Packets.Formatter;
using NetworkSocketServer.NetworkLayer.Core.TransportHandler.Tcp;

namespace NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler.Factory
{
    class AcceptedRetryClientTransportHandlerFactory : IClientTransportHandlerFactory
    {
        private readonly ClientTransportHandlerRetrySettings _retrySettings;

        public AcceptedRetryClientTransportHandlerFactory(ClientTransportHandlerRetrySettings retrySettings)
        {
            _retrySettings = retrySettings;
        }

        public IClientTransportHandler CreateTransportHandler()
        {
            var transportHandler = new TcpBlockingReceiveTransportHandler(
                new ManualPacketByteFormatter(), 
                PacketConstants.PacketThresholdSize);

            return new ClientTransportHandlerWithRetry(
                new ClientTransportHandlerWithPacketChecking(
                    new DirectClientTransportHandler(transportHandler)),
                _retrySettings,
                new ConsoleLogger());
        }
    }
}
