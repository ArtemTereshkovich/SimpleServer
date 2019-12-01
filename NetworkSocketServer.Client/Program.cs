using System;
using NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;
using NetworkSocketServer.TransportLayer.Client.ClientManager;
using NetworkSocketServer.TransportLayer.Client.ConnectionManager;
using NetworkSocketServer.TransportLayer.Client.RequestExecutor;
using NetworkSocketServer.TransportLayer.Client.ServiceHandlers.RequestExecutor;
using NetworkSocketServer.TransportLayer.Client.TransportManager;

namespace NetworkSocketServer.Client
{
    class Program
    {
        static void Main()
        {
            var keepAliveOptions = new SocketKeepAliveOptions
            {
                KeepAliveTime = 5000,
                KeepAliveInterval = 5000,
            };

            var retrySettings = new RetrySettings
            {
                CountReconnect = 5,
                ReconnectPeriod = TimeSpan.FromSeconds(10),
                TimeOutAnswer = TimeSpan.FromSeconds(4)
            };

            var networkClientManagerFactory = new ClientConnectionManagerFactory(
                new ConnectorDispatcherFactory(), 
                new ClientTransportManagerFactory(retrySettings));
            
            new Client(networkClientManagerFactory, keepAliveOptions)
                .Run().Wait();
        }
    }
}
