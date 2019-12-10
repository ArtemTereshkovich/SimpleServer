namespace NetworkSocketServer.TransportLayer.Core.Serializer
{
    public interface IBytesSerializer
    {
        byte[] Serialize<T>(T serializeObject);

        T Deserialize<T>(byte[] array);
    }
}
