namespace NetworkSocketServer.TransportLayer.Serializer
{
    public interface IByteSerializer
    {
        byte[] Serialize<T>(T serializeObject);

        T Deserialize<T>(byte[] array);
    }
}
