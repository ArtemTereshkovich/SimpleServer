using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using NetworkSocketServer.TransportLayer.DTO;

namespace NetworkSocketServer.TransportLayer.Serializer
{
    class BinaryFormatterByteSerializer : IByteSerializer
    {
        private readonly object _lockObject;

        public BinaryFormatterByteSerializer()
        {
            _lockObject = new object();
        }
        public byte[] SerializeT<T>(T serializeObject)
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
            byte[] size = BitConverter.GetBytes(packet.Size);
            byte[] offset = BitConverter.GetBytes(packet.Offset);
            byte[] payload = packet.Payload;

            return server
                .Concat(client)
                .Concat(guid)
                .Concat(size)
                .Concat(offset)
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
                Size = BitConverter.ToInt32(array.Skip(24).Take(4).ToArray()),
                Offset = BitConverter.ToInt32(array.Skip(28).Take(4).ToArray()),
                Payload = array.Skip(32).ToArray()
            };
        }

        public T DeserializeT<T>(byte[] array)
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
