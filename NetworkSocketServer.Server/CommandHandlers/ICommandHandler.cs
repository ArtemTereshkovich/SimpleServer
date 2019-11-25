using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;

namespace NetworkSocketServer.Server.CommandHandlers
{
    internal  interface ICommandHandler
    {
        Task<Response> Handle(Request request);
    }
}
