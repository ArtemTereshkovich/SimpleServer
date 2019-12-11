using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Client.Connectors;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;

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
