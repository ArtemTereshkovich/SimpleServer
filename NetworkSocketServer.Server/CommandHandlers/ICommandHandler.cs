using System.Threading.Tasks;
using NetworkSocketServer.Messages;

namespace NetworkSocketServer.Server.CommandHandlers
{
    internal  interface ICommandHandler
    {
        Task Handle(Command command);
    }
}
