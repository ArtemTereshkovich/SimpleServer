using System;

namespace NetworkSocketServer.DTO.Responses
{
    [Serializable]
    public class UploadFileResponse : Response
    {
        public string Filename { get; set; }

        public TimeSpan UploadTime { get; set; }
    }
}
