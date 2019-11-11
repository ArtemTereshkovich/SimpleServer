using System;

namespace NetworkSocketServer.Messages
{

    [Serializable]
    public class Message
    {
        public string ClientId { get; set; }
        public MessageType MessageType { get; set; }
    }
}
