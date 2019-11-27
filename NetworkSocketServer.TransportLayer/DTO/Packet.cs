using System;

namespace NetworkSocketServer.TransportLayer.DTO
{
    [Serializable]
    public class Packet
    {
        public Guid SessionId { get; set; }

        public int Size { get; set; }

        public int Offset { get; set; }
        
        public PacketClientCommand PacketClientCommand { get; set; }

        public PacketServerResponse PacketServerResponse { get; set; }

        public byte[] Payload { get; set; }
    }
}
