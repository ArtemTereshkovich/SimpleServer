namespace NetworkSocketServer.TransportLayer.DTO
{
    public enum PacketClientCommand
    {
        Read,
        Write,
        ExecuteBuffer,
        ExecutePayload,
        Close,
        None
    }
}
