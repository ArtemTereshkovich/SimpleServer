namespace NetworkSocketServer.TransportLayer.Packets
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
