using System;

namespace NetworkSocketServer.Messages
{

    [Serializable]
    public class Command
    {
        public string ClientId { get; set; }
        public CommandType CommandType { get; set; }
    }
}
