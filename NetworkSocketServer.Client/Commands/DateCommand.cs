namespace NetworkSocketServer.Client.Commands
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
