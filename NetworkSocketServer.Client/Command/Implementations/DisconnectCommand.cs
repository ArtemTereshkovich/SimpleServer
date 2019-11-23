using NetworkSocketServer.Client.Command.Interfaces;

namespace NetworkSocketServer.Client.Command.Implementations
{
    public class DisconnectCommand : ICommand
    {
        public const string Command = "-disconnect";

        public void Execute(CommandExecutor executor)
        {
            executor.Execute(this);
        }

        public static DisconnectCommand Parse(string _)
        {
            return new DisconnectCommand();
        }
    }
}
