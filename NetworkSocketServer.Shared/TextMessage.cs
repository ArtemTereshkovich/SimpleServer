using System;

namespace NetworkSocketServer.Messages
{
    [Serializable]
    public class TextMessage : Message
    {
        public string Text { get; set; }
    }
}
