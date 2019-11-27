using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.NetworkLayer.Connectors;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.NetworkClientManager
{
    public interface INetworkClientManager
    {
        bool IsConnected { get; }

        Task Connect(NetworkConnectorSettings connectSettings);

        Task<Response> HandleRequest(Request request);

        Task Disconnect();
    }
}
