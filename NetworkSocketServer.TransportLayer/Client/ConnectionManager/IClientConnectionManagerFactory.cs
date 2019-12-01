using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;
using NetworkSocketServer.TransportLayer.Client.ConnectionManager;

namespace NetworkSocketServer.TransportLayer.Client.ClientManager
{
    public interface IClientConnectionManagerFactory
    {
        IClientConnectionManager Create(SocketKeepAliveOptions socketKeepAliveOptions);
    }
}
