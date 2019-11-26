using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.NetworkLayer
{
    public interface IConnectionManager
    {
        Task HandleNewConnection(ITransportHandler transportHandler);
    }
}
