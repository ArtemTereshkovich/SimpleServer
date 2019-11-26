using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.Connectors
{
    internal interface INetworkConnector
    {
        Task Activate(ITransportHandler transportHandler);
    }
}
