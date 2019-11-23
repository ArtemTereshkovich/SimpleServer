using NetworkSocketServer.Client.Command.Interfaces;

namespace NetworkSocketServer.Client.Command.Implementations
{
    public class DownloadCommand : ICommand
    {
        public const string Command = "-download";

        public string FileName { get; set; }


        public void Execute(CommandExecutor executor)
        {
            executor.Execute(this);
        }

        public static DownloadCommand Parse(string data)
        {
            return new DownloadCommand()
            {
                FileName = data
            };
        }
    }
}
