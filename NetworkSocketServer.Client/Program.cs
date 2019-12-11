using System;
using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;
using NetworkSimpleServer.NetworkLayer.Client.ConnectorDispatcher;
using NetworkSimpleServer.NetworkLayer.Core.SocketOptionsAccessor.KeepAlive;
using NetworkSocketServer.TransportLayer.Client.ConnectionManager;
using NetworkSocketServer.TransportLayer.Client.TransportManager;

namespace NetworkSocketServer.Client
{
    class Program
    {
        static void Main()
        {
            var keepAliveOptions = new SocketKeepAliveOptions
            {
                KeepAliveTime = 30000,
                KeepAliveInterval = 30000,
            };

            var retrySettings = new ClientTransportHandlerRetrySettings
            {
                CountReconnect = 5,
                ReconnectPeriod = TimeSpan.FromSeconds(15),
                TimeOutAnswer = TimeSpan.FromSeconds(5)
            };

            var networkClientManagerFactory = new ClientConnectionManagerFactory(
                new SynchronousConnectorDispatcherFactory(retrySettings), 
                new ClientTransportManagerFactory());

            new Client(networkClientManagerFactory, keepAliveOptions)
                .Run().Wait();
        }
    }
}
