using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.NetworkLayer.Connectors;

namespace NetworkSocketServer.TransportLayer.Client.ConnectionManager
{
    public interface IClientConnectionManager
    {
        bool IsConnected { get; }

        Task Connect(NetworkConnectorSettings connectSettings);

        Task<Response> SendRequest(Request request);

        Task Disconnect();
    }
}
