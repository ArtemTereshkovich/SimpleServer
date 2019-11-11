namespace NetworkSocketServer.Network.ConnectionDispatcher
{
    internal interface INetworkAcceptorSubscriber
    {
        void RegisterAcceptor(INetworkAcceptor acceptor);
    }
}
