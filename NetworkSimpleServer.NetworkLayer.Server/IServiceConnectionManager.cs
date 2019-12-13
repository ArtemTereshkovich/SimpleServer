using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.Server
{
    public interface IServiceConnectionManager
    {
        Task HandleNewConnection(ITransportHandler transportHandler);
    }
}
