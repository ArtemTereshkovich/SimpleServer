using System;

namespace NetworkSocketServer.DTO.Requests
{
    [Serializable]
    public class DownloadFileRequest : Request
    {
        public string Filename { get; set; }
    }
}
