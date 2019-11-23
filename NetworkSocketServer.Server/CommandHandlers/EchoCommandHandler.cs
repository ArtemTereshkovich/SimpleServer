using System.Threading.Tasks;
using NetworkSocketServer.Commands;
using NetworkSocketServer.Messages;
using NetworkSocketServer.Network.TransportHandler;

namespace NetworkSocketServer.Server.CommandHandlers
{
    internal class EchoCommandHandler : ICommandHandler
    {
        private readonly ITransportHandler _transportHandler;

        public EchoCommandHandler(ITransportHandler transportHandler)
        {
            _transportHandler = transportHandler;
        }

        public async Task Handle(Command command)
        {

            var response = new TextCommand
            {
                CommandType = CommandType.TextResponse,
                Text = (command as TextCommand)?.Text
            };

            await _transportHandler.Send(response.Serialize());
        }
    }
}