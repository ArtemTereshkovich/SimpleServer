using System;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;

namespace NetworkSocketServer.Server.ServiceHandlers
{
    class DateServiceHandler : IRequestHandler
    {
        public Task<Response> HandleDateRequest(DateRequest request)
        {
            var response = new DateResponse()
            {
                ResponseId = request.RequestId,
                Offset = request.ClientDate - DateTime.Now,
                ServerTime = DateTime.Now
            };

            return Task.FromResult((Response)response);
        }

        public async Task<Response> HandleRequest(Request request)
        {
            if (request is DateRequest dateRequest)
            {
                return await HandleDateRequest(dateRequest);
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
    }
}
