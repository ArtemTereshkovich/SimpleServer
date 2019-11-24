using System;

namespace NetworkSocketServer.Commands
{

    [Serializable]
    public class Command
    {
        public string ClientId { get; set; }
        public CommandType CommandType { get; set; }
    }
}
