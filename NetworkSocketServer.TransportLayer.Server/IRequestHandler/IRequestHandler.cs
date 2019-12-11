using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;

namespace NetworkSocketServer.TransportLayer.Server.IRequestHandler
{
    public interface IRequestHandler
    {
        Task<Response> HandleRequest(Request request);
    }
}