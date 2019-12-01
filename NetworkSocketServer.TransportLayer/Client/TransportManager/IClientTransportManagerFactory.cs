using NetworkSocketServer.TransportLayer.Client.ConnectionManager;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager
{
    public interface IClientTransportManagerFactory
    {
        IClientTransportManager Create(ClientConnectionManager clientConnectionManager);
    }
}
