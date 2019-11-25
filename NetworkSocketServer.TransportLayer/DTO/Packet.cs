using System;

namespace NetworkSocketServer.TransportLayer.DTO
{
    [Serializable]
    class Packet
    {
        public Guid ConnectionId { get; set; }

        public long Position { get; set; }

        public long Offset { get; set; }
        
        public PacketCommand PacketCommand { get; set; }

        public byte[] Payload { get; set; }
    }
}
