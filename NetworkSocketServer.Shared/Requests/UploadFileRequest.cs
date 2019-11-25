using System;

namespace NetworkSocketServer.DTO.Requests
{
    [Serializable]
    public class UploadFileRequest : Request
    {
        public string FileName { get; set; }

        public long Size { get; set; }

        public byte[] File { get; set; }
    }
}
