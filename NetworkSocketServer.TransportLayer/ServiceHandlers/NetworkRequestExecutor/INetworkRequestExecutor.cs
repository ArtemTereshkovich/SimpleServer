using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.NetworkLayer.Connectors;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.NetworkRequestExecutor
{
    public interface INetworkRequestExecutor
    {
        bool IsConnected { get; }

        Task Connect(NetworkConnectorSettings connectSettings);

        Task<Response> HandleResponse(Request request);

        Task Disconnect();
    }
}
