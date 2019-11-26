using System.Threading.Tasks;

namespace NetworkSocketServer.Client.Commands
{
    public class UploadFileCommand : ICommand
    {
        public const string Command = "-upload";

        public string FileName { get; set; }


        public async Task Execute(CommandExecutor executor)
        {
            await executor.Execute(this);
        }

        public static UploadFileCommand Parse(string data)
        { 
            return new UploadFileCommand()
            {
                FileName = data
            };
        }
    }
}
