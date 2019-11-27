using NetworkSocketServer.TransportLayer.DTO;

namespace NetworkSocketServer.TransportLayer.Serializer
{
    public interface IByteSerializer
    {
        byte[] SerializeT<T>(T serializeObject);

        T DeserializeT<T>(byte[] array);

        byte[] Serialize(Packet serializeObject);

        Packet Deserialize(byte[] array);
    }
}
