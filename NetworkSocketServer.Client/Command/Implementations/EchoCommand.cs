using NetworkSocketServer.Client.Command.Interfaces;

namespace NetworkSocketServer.Client.Command.Implementations
{
    public class EchoCommand : ICommand
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

        public void Execute(CommandExecutor executor)
        {
            executor.Execute(this);
        }

        public override string ToString()
        {
            return $"{Command} {Message}";
        }
    }
}
