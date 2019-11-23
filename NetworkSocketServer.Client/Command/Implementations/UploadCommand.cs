using NetworkSocketServer.Client;
using NetworkSocketServer.Client.Command.Interfaces;

namespace SPOLKS.Client.Command.Implementations
{
    public class UploadCommand : ICommand
    {
        public const string Command = "-upload";

        public string FileName { get; set; }


        public void Execute(CommandExecutor executor)
        {
            executor.Execute(this);
        }

        public static UploadCommand Parse(string data)
        { 
            return new UploadCommand()
            {
                FileName = data
            };
        }
    }
}
