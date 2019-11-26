namespace NetworkSocketServer.Client.Commands
{
    public class DownloadFileCommand : ICommand
    {
        public const string Command = "-download";

        public string FileName { get; set; }
        
        public void Execute(CommandExecutor executor)
        {
            executor.Execute(this);
        }

        public static DownloadFileCommand Parse(string data)
        {
            return new DownloadFileCommand()
            {
                FileName = data
            };
        }
    }
}
