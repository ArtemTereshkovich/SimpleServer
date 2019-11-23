using System;
using System.Threading.Tasks;
using NetworkSocketServer.Commands;
using NetworkSocketServer.Messages;
using NetworkSocketServer.Network.TransportHandler;

namespace NetworkSocketServer.Server.CommandHandlers
{
    internal class TimeCommandHandler : ICommandHandler
    {
        private readonly ITransportHandler _transportHandler;

        public TimeCommandHandler(ITransportHandler transportHandler)
        {
            _transportHandler = transportHandler;
        }

        public async Task Handle(Command command)
        {
            var response = new TextCommand
            {
                CommandType = CommandType.TextResponse,
                Text = $"{DateTime.Now:dddd, dd MMMM yyyy HH:mm:ss}"
            };

            await _transportHandler.Send(response.Serialize());
        }
    }
}