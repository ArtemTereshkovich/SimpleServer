namespace NetworkSocketServer.Client.Commands
{
    public interface ICommand
    {
        void Execute(CommandExecutor executor);
    }
}
