using System.Collections.Generic;
using NetworkSocketServer.NetworkLayer.Acceptors;
using NetworkSocketServer.NetworkLayer.Dispatchers.AcceptorDispatcher;
using NetworkSocketServer.NetworkLayer.Server;

namespace NetworkSocketServer.NetworkLayer
{
    class SimpleServer : IServer
    {
        private readonly IAcceptorDispatcher _acceptorDispatcher;
        private readonly IEnumerable<INetworkAcceptor> _networkAcceptors;

        public SimpleServer(
            IAcceptorDispatcher acceptorDispatcher,
            IEnumerable<INetworkAcceptor> networkAcceptors)
        {
            _acceptorDispatcher = acceptorDispatcher;
            _networkAcceptors = networkAcceptors;
        }

        public void StartHost()
        {
            foreach (var acceptor in _networkAcceptors)
            {
                acceptor.Open();
            }

            _acceptorDispatcher.StartListen();
        }

        public void StopHost()
        {
            foreach (var acceptor in _networkAcceptors)
            {
                acceptor.Close();
            }

            _acceptorDispatcher.StopListen();
        }
    }
}
