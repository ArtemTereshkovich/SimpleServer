namespace NetworkSocketServer.Network
{
    internal interface IConnectionDispatcher
    {
        void RegisterAcceptor(INetworkAcceptor acceptor);
    }
}
