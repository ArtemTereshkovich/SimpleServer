namespace NetworkSocketServer.TransportLayer.Messages
{
    public class ContentMessage
    {
        public int PacketNumber { get; set; }

        public int PacketLeft { get; set; }

        public byte[] Content { get; set; }
    }
}
