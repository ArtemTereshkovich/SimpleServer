using System.Threading.Tasks;

namespace NetworkSocketServer.Client.Commands
{
    public class UploadFileCommand : ICommand
    {
        public const string Command = "-upload";

        public string FileName { get; set; }


        public Task Execute(CommandExecutor executor)
        {
            executor.Execute(this);

            return Task.CompletedTask;
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
