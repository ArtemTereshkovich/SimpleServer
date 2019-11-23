namespace NetworkSocketServer.Network.SocketOptionsAccessor
{
    internal interface ISocketOptionsAccessorFactory
    {
        ISocketOptionsAccessor GetSocketOptionsAccessor();
    }
}
