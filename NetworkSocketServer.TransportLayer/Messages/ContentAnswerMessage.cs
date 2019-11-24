namespace NetworkSocketServer.TransportLayer.Messages
{
    public class ContentAnswerMessage
    {
        public int PacketReceive { get; set; }

        public bool IsRequiredResend { get; set; }
    }
}
