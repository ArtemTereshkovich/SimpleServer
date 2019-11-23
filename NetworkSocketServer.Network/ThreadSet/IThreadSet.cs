using System;

namespace NetworkSocketServer.Network.ThreadSet
{
    internal interface IThreadSet
    {
        void Execute(Action<INetworkAcceptor> action, INetworkAcceptor acceptor);
    }
}
