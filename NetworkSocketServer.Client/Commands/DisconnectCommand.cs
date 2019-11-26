using System.Threading.Tasks;

namespace NetworkSocketServer.Client.Commands
{
    public class DisconnectCommand : ICommand
    {
        public const string Command = "-disconnect";

        public async Task Execute(CommandExecutor executor)
        {
            await executor.Execute(this);
        }

        public static DisconnectCommand Parse(string _)
        {
            return new DisconnectCommand();
        }
    }
}
