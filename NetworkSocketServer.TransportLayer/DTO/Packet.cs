using System;

namespace NetworkSocketServer.TransportLayer.DTO
{
    [Serializable]
    class Packet
    {
        public Guid ConnectionId { get; set; }

        public int Position { get; set; }

        public int Offset { get; set; }
        
        public PacketClientCommand PacketClientCommand { get; set; }

        public PacketServerResponse PacketServerResponse { get; set; }

        public byte[] Payload { get; set; }
    }
}
