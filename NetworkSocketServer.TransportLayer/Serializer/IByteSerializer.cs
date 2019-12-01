using NetworkSocketServer.TransportLayer.Packets;

namespace NetworkSocketServer.TransportLayer.Serializer
{
    public interface IByteSerializer
    {
        byte[] Serialize<T>(T serializeObject);

        T Deserialize<T>(byte[] array);

        byte[] Serialize(Packet serializeObject);

        Packet Deserialize(byte[] array);
    }
}
