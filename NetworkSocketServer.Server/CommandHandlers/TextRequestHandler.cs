using System;
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
                ResponseId = fileRequest.RequestId,
                Text = fileRequest.Text,
            };

            Console.WriteLine("Receive text request:  " + response.Text);

            return Task.FromResult((Response)response);
        }
    }
}