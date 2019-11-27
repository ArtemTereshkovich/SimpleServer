using System;
using NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;
using NetworkSocketServer.TransportLayer.ServiceHandlers.NetworkClientManager;
using NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor;

namespace NetworkSocketServer.Client
{
    class Program
    {
        static void Main()
        {
            var keepAliveOptions = new SocketKeepAliveOptions
            {
                KeepAliveTime = 90000,
                KeepAliveInterval = 90000,
            };

            var retrySettings = new RetrySettings
            {
                CountReconnect = 5,
                ReconnectPeriod = TimeSpan.FromSeconds(10),
                TimeOutAnswer = TimeSpan.FromSeconds(90)
            };

            var networkClientManagerFactory = new NetworkClientManagerFactory(
                new ConnectorDispatcherFactory(), 
                new RequestExecutorFactory(retrySettings));
            
            new Client(networkClientManagerFactory, keepAliveOptions)
                .Run().Wait();
        }
    }
}
