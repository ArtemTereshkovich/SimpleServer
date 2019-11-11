using NetworkSocketServer.Client;

namespace SPOLKS.Client.Command
{
    public class DisconnectCommand : Implementations.Command
    {
        public const string Command = "-disconnect";

        public override void Execute(CommandExecutor executor)
        {
            executor.Execute(this);
        }

        public static DisconnectCommand Parse(string _)
        {
            return new DisconnectCommand();
        }
    }
}
