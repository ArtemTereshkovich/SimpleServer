using NetworkSocketServer.Client;

namespace SPOLKS.Client.Command
{
    public class TimeCommand : Implementations.Command
    {
        public const string Command = "-time";
        public static TimeCommand Parse(string _)
        {
            return new TimeCommand();
        }

        public override void Execute(CommandExecutor executor)
        {
            executor.Execute(this);
        }

        public override string ToString()
        {
            return $"{Command}";
        }
    }
}
