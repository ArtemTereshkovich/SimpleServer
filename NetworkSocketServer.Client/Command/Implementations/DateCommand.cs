using NetworkSocketServer.Client.Command.Interfaces;

namespace NetworkSocketServer.Client.Command.Implementations
{
    public class DateCommand : ICommand
    {
        public const string Command = "-time";
        public static DateCommand Parse(string _)
        {
            return new DateCommand();
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
