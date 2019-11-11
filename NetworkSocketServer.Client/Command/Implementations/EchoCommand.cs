using NetworkSocketServer.Client;

namespace SPOLKS.Client.Command
{
    public class EchoCommand : Implementations.Command
    {
        public const string Command = "-echo";
        public string Message { get; private set; }
        public static EchoCommand Parse(string data)
        {
            return new EchoCommand()
            {
                Message = data
            };
        }

        public override void Execute(CommandExecutor executor)
        {
            executor.Execute(this);
        }

        public override string ToString()
        {
            return $"{Command} {Message}";
        }
    }
}
