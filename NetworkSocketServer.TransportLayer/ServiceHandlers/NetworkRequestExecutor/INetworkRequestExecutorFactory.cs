using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.NetworkRequestExecutor
{
    public interface INetworkRequestExecutorFactory
    {
        INetworkRequestExecutor CreateExecutor(SocketKeepAliveOptions socketKeepAliveOptions);
    }
}
