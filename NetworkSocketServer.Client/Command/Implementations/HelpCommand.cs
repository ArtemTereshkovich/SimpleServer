using NetworkSocketServer.Client.Command.Interfaces;

namespace NetworkSocketServer.Client.Command.Implementations
{
    public class HelpCommand : ICommand
    {
        public const string Command = "-help";

        public void Execute(CommandExecutor executor)
        {
            executor.Execute(this);
        }

        public static HelpCommand Parse(string _)
        {
            return new HelpCommand();
        }
    }
}
