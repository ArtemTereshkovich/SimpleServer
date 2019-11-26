using System;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.NetworkRequestExecutor
{
    public class SimpleNetworkExecutorFactory : INetworkRequestExecutorFactory
    {
        public INetworkRequestExecutor CreateExecutor(SocketKeepAliveOptions socketKeepAliveOptions)
        {
            throw new NotImplementedException();
        }
    }
}
