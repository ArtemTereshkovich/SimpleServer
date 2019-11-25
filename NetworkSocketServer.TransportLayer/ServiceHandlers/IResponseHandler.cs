using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;

namespace NetworkSocketServer.TransportLayer
{
    interface IResponseHandler
    {
        Task<Request> HandleResponse(Response response);
    }
}
