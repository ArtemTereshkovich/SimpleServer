using System;

namespace NetworkSocketServer.DTO.Requests
{
    [Serializable]
    public class TextRequest : Request
    {
        public string Text { get; set; }
    }
}
