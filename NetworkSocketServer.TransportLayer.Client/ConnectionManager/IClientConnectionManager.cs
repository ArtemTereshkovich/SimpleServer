using NetworkSimpleServer.NetworkLayer.Client.Connectors;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;

namespace NetworkSocketServer.TransportLayer.Client.ConnectionManager
{
    public interface IClientConnectionManager
    {
        bool IsConnected { get; }

        void Connect(NetworkConnectorSettings connectSettings);

        Response SendRequest(Request request);

        void Disconnect();
    }
}
