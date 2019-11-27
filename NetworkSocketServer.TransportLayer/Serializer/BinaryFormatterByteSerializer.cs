using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetworkSocketServer.TransportLayer.Serializer
{
    public class BinaryFormatterByteSerializer : IByteSerializer
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
                using var memoryStream = new MemoryStream
                {
                    Position = 0
                };
                formatter.Serialize(memoryStream, serializeObject);
                return memoryStream.ToArray();
            }
        }

        public T Deserialize<T>(byte[] array)
        {
            lock (_lockObject)
            {
                var formatter = new BinaryFormatter();
                using var memoryStream = new MemoryStream(array)
                {
                    Position = 0
                };
                return (T) formatter.Deserialize(memoryStream);
            }
        }
    }
}
