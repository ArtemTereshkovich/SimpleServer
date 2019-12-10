using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using NetworkSocketServer.TransportLayer.Packets;

namespace NetworkSocketServer.TransportLayer.Serializer
{
    class BinaryFormatterByteSerializer : IByteSerializer
    {
        private readonly object _lockObject;

        public BinaryFormatterByteSerializer()
        {
            _lockObject = new object();
        }
        public byte[] Serialize<T>(T serializeObject)
        {
            lock (_lockObject)
            {
                if (serializeObject == null) return null;

                var formatter = new BinaryFormatter();
                using (var memoryStream = new MemoryStream())
                {
                    formatter.Serialize(memoryStream, serializeObject);
                    return memoryStream.ToArray();
                }
            }
        }

        public byte[] Serialize(Packet packet)
        {
            byte[] server = BitConverter.GetBytes((int) packet.PacketServerResponse);
            byte[] client = BitConverter.GetBytes((int) packet.PacketClientCommand);
            byte[] guid = packet.SessionId.ToByteArray();
            byte[] bufferSize = BitConverter.GetBytes(packet.BuffferSize);
            byte[] bufferOffset = BitConverter.GetBytes(packet.BufferOffset);
            byte[] payloadSize = BitConverter.GetBytes(packet.PayloadSize);
            byte[] payload = packet.Payload;

            

            return server
                .Concat(client)
                .Concat(guid)
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
                PacketServerResponse = (PacketServerResponse) BitConverter.ToInt32(array.Skip(0).Take(4).ToArray()),
                PacketClientCommand = (PacketClientCommand) BitConverter.ToInt32(array.Skip(4).Take(4).ToArray()),
                SessionId = new Guid(array.Skip(8).Take(16).ToArray()),
                BuffferSize = BitConverter.ToInt32(array.Skip(24).Take(4).ToArray()),
                BufferOffset = BitConverter.ToInt32(array.Skip(28).Take(4).ToArray()),
                PayloadSize = BitConverter.ToInt32(array.Skip(32).Take(4).ToArray()),
                Payload = array.Skip(36).ToArray()
            };
        }

        public T Deserialize<T>(byte[] array)
        {
            lock (_lockObject)
            {
                var formatter = new BinaryFormatter();

                using (var memoryStream = new MemoryStream())
                {
                    memoryStream.Write(array, 0, array.Length);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    return (T) formatter.Deserialize(memoryStream);
                }
            }
        }
    }
}
