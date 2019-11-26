using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;
using NetworkSocketServer.TransportLayer.ServiceHandlers.NetworkRequestExecutor;

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

            var networkExecutorFactory = new SimpleNetworkExecutorFactory();
            
            new Client(networkExecutorFactory, keepAliveOptions)
                .Run().Wait();
        }
    }
}
