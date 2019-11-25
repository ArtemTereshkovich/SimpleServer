using System;

namespace NetworkSocketServer.DTO.Responses
{
    [Serializable]
    public class TextResponse : Response
    {
        public string Text { get; set; }
    }
}
