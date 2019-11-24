using System.Threading.Tasks;
using NetworkSocketServer.Commands;

namespace NetworkSocketServer.Server.CommandHandlers
{
    internal  interface ICommandHandler
    {
        Task Handle(Command command);
    }
}
