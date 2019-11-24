using System;
using NetworkSocketServer.NetworkLayer.Acceptors;

namespace NetworkSocketServer.NetworkLayer.ThreadSet
{
    internal interface IThreadSet
    {
        void Execute(Action<INetworkAcceptor> action, INetworkAcceptor acceptor);
    }
}
