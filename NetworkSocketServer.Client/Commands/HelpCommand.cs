using System.Threading.Tasks;

namespace NetworkSocketServer.Client.Commands
{
    public class HelpCommand : ICommand
    {
        public const string Command = "-help";

        public Task Execute(CommandExecutor executor)
        {
            executor.Execute(this);

            return Task.CompletedTask;
        }

        public static HelpCommand Parse(string _)
        {
            return new HelpCommand();
        }
    }
}
