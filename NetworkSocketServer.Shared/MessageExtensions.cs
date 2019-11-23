using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetworkSocketServer.Commands
{
    public static class ObjectExtensions
    {
        public static byte[] Serialize(this object obj)
        {
            if (obj == null) return null;

            var formatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        public static T Deserialize<T>(this byte[] bytes)
        {
            var formatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(bytes))
            {
                return (T)formatter.Deserialize(memoryStream);
            }
        }
    }
}
