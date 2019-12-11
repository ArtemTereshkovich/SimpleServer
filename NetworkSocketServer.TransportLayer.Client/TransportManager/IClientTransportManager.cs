using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager
{
    public interface IClientTransportManager
    {
        Response SendRequest(Request request);
    }
}
