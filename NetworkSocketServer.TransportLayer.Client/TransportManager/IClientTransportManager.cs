using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager
{
    public interface IClientTransportManager
    {
        Task<Response> SendRequest(Request request);
    }
}
