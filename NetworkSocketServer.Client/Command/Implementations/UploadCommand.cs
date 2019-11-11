using NetworkSocketServer.Client;

namespace SPOLKS.Client.Command.Implementations
{
    public class UploadCommand : Implementations.Command
    {
        public const string Command = "-upload";

        public string FileName { get; set; }


        public override void Execute(CommandExecutor executor)
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
