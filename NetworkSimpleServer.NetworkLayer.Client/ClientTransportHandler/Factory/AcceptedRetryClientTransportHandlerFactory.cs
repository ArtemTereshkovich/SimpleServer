﻿using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler.Decorators;
using NetworkSimpleServer.NetworkLayer.Client.Connectors;
using NetworkSimpleServer.NetworkLayer.Core;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.Packets.Serializer;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Tcp;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Udp;

namespace NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler.Factory
{
    class AcceptedRetryClientTransportHandlerFactory : IClientTransportHandlerFactory
    {
        private readonly ClientTransportHandlerRetrySettings _retrySettings;

        public AcceptedRetryClientTransportHandlerFactory(ClientTransportHandlerRetrySettings retrySettings)
        {
            _retrySettings = retrySettings;
        }

        public IClientTransportHandler CreateTransportHandler(ConnectionType connectionType)
        {
            ITransportHandler transportHandler = null;

            if (connectionType == ConnectionType.Udp)
            {
                transportHandler = new UdpCycledCheckTransportHandler(new ManualPacketSerializer(), PacketConstants.PacketThresholdSize);
            }
            else
            {
                transportHandler = new TcpBlockingReceiveTransportHandler(new ManualPacketSerializer(), PacketConstants.PacketThresholdSize);
            }

            return new ClientTransportHandlerWithRetry(
                new ClientTransportHandlerWithPacketChecking(new DirectClientTransportHandler(transportHandler)),
                _retrySettings,
                new ConsoleLogger());
        }
    }
}
