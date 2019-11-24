using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.NetworkLayer
{
    public interface INewTransportHandler
    {
        Task HandleNewConnection(ITransportHandler transportHandler);
    }
}
