using NetworkSocketServer.Client;

namespace SPOLKS.Client.Command
{
    public class DownloadCommand : Implementations.Command
    {
        public const string Command = "-download";

        public string FileName { get; set; }


        public override void Execute(CommandExecutor executor)
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
