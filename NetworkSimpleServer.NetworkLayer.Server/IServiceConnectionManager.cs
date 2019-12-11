using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;

namespace NetworkSimpleServer.NetworkLayer.Server
{
    public interface IServiceConnectionManager
    {
        Task HandleNewConnection(ITransportHandler transportHandler);

        Task ProcessRegistered();
    }
}
