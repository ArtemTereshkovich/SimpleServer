using NetworkSocketServer.Client;

namespace SPOLKS.Client.Command.Interfaces
{
    public interface ICommand
    {
        void Execute(CommandExecutor executor);
    }
}
