using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetworkSocketServer.TransportLayer.Core.Serializer
{
    public class BinaryFormatterBytesSerializer : IBytesSerializer
    {
        public BinaryFormatterBytesSerializer()
        {
        }

        public byte[] Serialize<T>(T serializeObject)
        {
            if (serializeObject == null) return null;

            var formatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, serializeObject);
                return memoryStream.ToArray();
            }
        }

        public T Deserialize<T>(byte[] array)
        {
            var formatter = new BinaryFormatter();

            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(array, 0, array.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);

                return (T)formatter.Deserialize(memoryStream);
            }
        }
    }
}
