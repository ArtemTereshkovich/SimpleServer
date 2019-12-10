using System.Collections.Generic;
using NetworkSimpleServer.NetworkLayer.Server.AcceptorDispatcher;
using NetworkSimpleServer.NetworkLayer.Server.Acceptors;

namespace NetworkSimpleServer.NetworkLayer.Server
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