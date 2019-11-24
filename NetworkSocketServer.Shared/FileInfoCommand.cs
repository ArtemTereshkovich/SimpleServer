using System;

namespace NetworkSocketServer.Commands
{
    [Serializable]
    public class FileInfoCommand : Command
    {
        public string FileName { get; set; }
        public bool IsExist { get; set; }
        public long Size { get; set; }
    }
}
