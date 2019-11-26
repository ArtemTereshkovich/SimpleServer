using System;

namespace NetworkSocketServer.DTO.Responses
{
    [Serializable]
    public class DownloadFileResponse : Response
    {
        public byte[] File { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public string Filename { get; set; }

        public long FileSize { get; set; }
    }
}
