using System;
using System.Linq;

namespace NetworkSimpleServer.NetworkLayer.Core.Packets.Serializer
{
    public class ManualPacketSerializer : IPacketSerializer
    {
        public byte[] Serialize(Packet packet)
        {
            byte[] server = BitConverter.GetBytes((int)packet.PacketServerResponse);
            byte[] client = BitConverter.GetBytes((int)packet.PacketClientCommand);
            byte[] guid = packet.SessionId.ToByteArray();
            byte[] packetId = packet.PacketId.ToByteArray();
            byte[] bufferSize = BitConverter.GetBytes(packet.BuffferSize);
            byte[] bufferOffset = BitConverter.GetBytes(packet.BufferOffset);
            byte[] payloadSize = BitConverter.GetBytes(packet.PayloadSize);
            byte[] payload = packet.Payload;



            return server
                .Concat(client)
                .Concat(guid)
                .Concat(packetId)
                .Concat(bufferSize)
                .Concat(bufferOffset)
                .Concat(payloadSize)
                .Concat(payload)
                .ToArray();
        }

        public Packet Deserialize(byte[] array)
        {
            return new Packet
            {
                PacketServerResponse = (PacketServerResponse)BitConverter.ToInt32(array.Skip(0).Take(4).ToArray()),
                PacketClientCommand = (PacketClientCommand)BitConverter.ToInt32(array.Skip(4).Take(4).ToArray()),
                SessionId = new Guid(array.Skip(8).Take(16).ToArray()),
                PacketId = new Guid(array.Skip(24).Take(16).ToArray()),
                BuffferSize = BitConverter.ToInt32(array.Skip(24).Take(4).ToArray()),
                BufferOffset = BitConverter.ToInt32(array.Skip(28).Take(4).ToArray()),
                PayloadSize = BitConverter.ToInt32(array.Skip(32).Take(4).ToArray()),
                Payload = array.Skip(36).ToArray()
            };
        }
    }
}
