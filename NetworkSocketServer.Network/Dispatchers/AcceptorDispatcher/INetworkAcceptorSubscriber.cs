using NetworkSocketServer.NetworkLayer.Acceptors;

namespace NetworkSocketServer.NetworkLayer.Dispatchers.AcceptorDispatcher
{
    internal interface INetworkAcceptorSubscriber
    {
        void RegisterAcceptor(INetworkAcceptor acceptor);
    }
}
