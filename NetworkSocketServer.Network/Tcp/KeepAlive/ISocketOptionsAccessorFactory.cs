namespace NetworkSocketServer.Network.Tcp.KeepAlive
{
    internal interface ISocketOptionsAccessorFactory
    {
        ISocketOptionsAccessor GetSocketOptionsAccessor();
    }
}
