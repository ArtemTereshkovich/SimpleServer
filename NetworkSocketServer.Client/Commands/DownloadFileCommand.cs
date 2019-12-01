using System.Threading.Tasks;

namespace NetworkSocketServer.Client.Commands
{
    public class DownloadFileCommand : ICommand
    {
        public const string Command = "-download";

        public string FileName { get; set; }
        
        public async Task Execute(CommandExecutor executor)
        {
            await executor.Execute(this);
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
