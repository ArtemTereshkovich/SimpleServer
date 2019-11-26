using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.Connectors;
using NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher
{
    public interface IConnectorDispatcher
    {
        Task<ITransportHandler> CreateTransportHandler(NetworkConnectorSettings settings);
    }
}
