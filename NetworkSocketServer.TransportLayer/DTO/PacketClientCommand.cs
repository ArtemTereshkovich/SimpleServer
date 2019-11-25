namespace NetworkSocketServer.TransportLayer.DTO
{
    enum PacketClientCommand
    {
        Read,
        Write,
        ExecuteBuffer,
        ExecutePayload,
        ClearBuffers,
        None
    }
}
