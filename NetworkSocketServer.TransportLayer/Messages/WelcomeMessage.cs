using System;

namespace NetworkSocketServer.TransportLayer.Messages
{
    public class WelcomeMessage
    {
        public Guid ClientId { get; set; }

        public MessageOptions MessageOptions { get; set; }

        public bool IsContinueReceive { get; set; }

        public bool IsContinueTransmit { get; set; }
    }
}
