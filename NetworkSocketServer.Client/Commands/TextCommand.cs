namespace NetworkSocketServer.Client.Commands
{
    public class TextCommand : ICommand
    {
        public const string Command = "-echo";
        public string Message { get; private set; }
        public static TextCommand Parse(string data)
        {
            return new TextCommand()
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
