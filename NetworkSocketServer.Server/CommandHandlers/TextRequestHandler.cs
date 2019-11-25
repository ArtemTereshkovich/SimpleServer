using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;

namespace NetworkSocketServer.Server.CommandHandlers
{
    internal class TextRequestHandler : ICommandHandler
    {
        public Task<Response> Handle(Request request)
        {
            var fileRequest = request as TextRequest;

            var response = new TextResponse
            {
                ConnectionId = fileRequest.ConnectionId,
                Text = fileRequest.Text,
            };

            return Task.FromResult((Response)response);
        }
    }
}