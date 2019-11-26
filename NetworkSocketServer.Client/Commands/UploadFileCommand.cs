namespace NetworkSocketServer.Client.Commands
{
    public class UploadFileCommand : ICommand
    {
        public const string Command = "-upload";

        public string FileName { get; set; }


        public void Execute(CommandExecutor executor)
        {
            executor.Execute(this);
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
