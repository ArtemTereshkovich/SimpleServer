namespace NetworkSimpleServer.NetworkLayer.Core.Packets
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
