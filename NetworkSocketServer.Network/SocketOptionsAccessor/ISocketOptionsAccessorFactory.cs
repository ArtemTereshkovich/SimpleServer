namespace NetworkSocketServer.NetworkLayer.SocketOptionsAccessor
{
    internal interface ISocketOptionsAccessorFactory
    {
        ISocketOptionsAccessor GetSocketOptionsAccessor();
    }
}
