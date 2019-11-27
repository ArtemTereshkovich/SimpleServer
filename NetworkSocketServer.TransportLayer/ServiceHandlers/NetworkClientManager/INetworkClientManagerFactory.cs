using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.NetworkClientManager
{
    public interface INetworkClientManagerFactory
    {
        INetworkClientManager Create(SocketKeepAliveOptions socketKeepAliveOptions);
    }
}
