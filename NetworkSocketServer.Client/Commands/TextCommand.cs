using System.Threading.Tasks;

namespace NetworkSocketServer.Client.Commands
{
    public class TextCommand : ICommand
    {
        public const string Command = "-text";
        public string Message { get; private set; }
        public static TextCommand Parse(string data)
        {
            return new TextCommand()
            {
                Message = data
            };
        }

        public Task Execute(CommandExecutor executor)
        {
            executor.Execute(this);

            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return $"{Command} {Message}";
        }
    }
}
