using System;

namespace NetworkSocketServer.Messages
{
    [Serializable]
    public class FileInfoMessage : Message
    {
        public string FileName { get; set; }
        public bool IsExist { get; set; }
        public long Size { get; set; }
    }
}
