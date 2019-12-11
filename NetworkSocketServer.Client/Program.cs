//using System;

//namespace NetworkSocketServer.Client
//{
//    class Program
//    {
//        static void Main()
//        {
//            var keepAliveOptions = new SocketKeepAliveOptions
//            {
//                KeepAliveTime = 30000,
//                KeepAliveInterval = 30000,
//            };

//            var retrySettings = new RetrySettings
//            {
//                CountReconnect = 5,
//                ReconnectPeriod = TimeSpan.FromSeconds(2),
//                TimeOutAnswer = TimeSpan.FromSeconds(4)
//            };

//            var networkClientManagerFactory = new ClientConnectionManagerFactory(
//                new ConnectorDispatcherFactory(), 
//                new ClientTransportManagerFactory(retrySettings));
            
//            new Client(networkClientManagerFactory, keepAliveOptions)
//                .Run().Wait();
//        }
//    }
//}
