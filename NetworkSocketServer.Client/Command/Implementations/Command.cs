using NetworkSocketServer.Client;
using SPOLKS.Client.Command.Interfaces;

namespace SPOLKS.Client.Command.Implementations
{
    public abstract class Command : ICommand
    {
        public abstract void Execute(CommandExecutor user);
    }
}
