using System;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;

namespace NetworkSocketServer.Server.CommandHandlers
{
    internal class DateRequestHandler : ICommandHandler
    {
        public Task<Response> Handle(Request request)
        {
            var dateRequest = request as DateRequest;

            var response = new DateResponse()
            {
                ResponseId = dateRequest.RequestId,
                Offset = dateRequest.ClientDate - DateTime.Now,
                ServerTime = DateTime.Now
            };

            return Task.FromResult((Response)response);
        }
    }
}