namespace NetworkSocketServer.Client.Command.Interfaces
{
    public interface ICommand
    {
        void Execute(CommandExecutor executor);
    }
}
