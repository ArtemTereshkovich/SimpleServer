using System.Threading.Tasks;
using NetworkSocketServer.Network.TransportHandler;

namespace NetworkSocketServer.Network
{
    public interface INetworkServiceHandler
    {
        Task HandleNewConnection(ITransportHandler transportHandler);
    }
}
