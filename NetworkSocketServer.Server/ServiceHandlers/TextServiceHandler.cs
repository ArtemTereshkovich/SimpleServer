using System;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;

namespace NetworkSocketServer.Server.ServiceHandlers
{
    class TextServiceHandler : IRequestHandler
    {
        public async Task<Response> HandleRequest(Request request)
        {
            if (request is TextRequest textRequest)
            {
                return await HandleTextRequest(textRequest);
            }
            else
            {
                return new ErrorResponse
                {
                    ErrorText = "Unsupported Command.",
                    ResponseId = request.RequestId,
                };
            }
        }

        private Task<TextResponse> HandleTextRequest(TextRequest textRequest)
        {
            var response = new TextResponse
            {
                ResponseId = textRequest.RequestId,
                Text = textRequest.Text,
            };

            Console.WriteLine("Receive text request:  " + response.Text);

            return Task.FromResult(response);
        }
    }
}
