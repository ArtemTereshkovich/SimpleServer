using NetworkSocketServer.Client.Command.Interfaces;

namespace NetworkSocketServer.Client.Command.Implementations
{
    public class TimeCommand : ICommand
    {
        public const string Command = "-time";
        public static TimeCommand Parse(string _)
        {
            return new TimeCommand();
        }

        public void Execute(CommandExecutor executor)
        {
            executor.Execute(this);
        }

        public override string ToString()
        {
            return $"{Command}";
        }
    }
}
