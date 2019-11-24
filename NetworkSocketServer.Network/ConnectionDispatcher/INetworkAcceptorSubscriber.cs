using NetworkSocketServer.NetworkLayer.Acceptors;

namespace NetworkSocketServer.NetworkLayer.ConnectionDispatcher
{
    internal interface INetworkAcceptorSubscriber
    {
        void RegisterAcceptor(INetworkAcceptor acceptor);
    }
}
