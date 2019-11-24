namespace NetworkSocketServer.TransportLayer.Serializer
{
    interface IByteSerializer
    {
        byte[] Serialize<T>(T serializeObject);

        T Deserialize<T>(byte[] array);
    }
}
