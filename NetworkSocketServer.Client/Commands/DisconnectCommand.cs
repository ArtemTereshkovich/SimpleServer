using System.Threading.Tasks;

namespace NetworkSocketServer.Client.Commands
{
    public class DisconnectCommand : ICommand
    {
        public const string Command = "-disconnect";

        public Task Execute(CommandExecutor executor)
        {
            executor.Execute(this);

            return Task.CompletedTask;
        }

        public static DisconnectCommand Parse(string _)
        {
            return new DisconnectCommand();
        }
    }
}
