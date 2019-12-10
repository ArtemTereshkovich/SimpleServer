using NetworkSimpleServer.NetworkLayer.Server.Acceptors;

namespace NetworkSimpleServer.NetworkLayer.Server.AcceptorDispatcher
{
    internal interface INetworkAcceptorSubscriber
    {
        void RegisterAcceptor(INetworkAcceptor acceptor);
    }
}
