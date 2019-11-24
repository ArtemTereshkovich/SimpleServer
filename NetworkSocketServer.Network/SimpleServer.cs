using System.Collections.Generic;
using NetworkSocketServer.NetworkLayer.Acceptors;
using NetworkSocketServer.NetworkLayer.ConnectionDispatcher;
using NetworkSocketServer.NetworkLayer.Server;

namespace NetworkSocketServer.NetworkLayer
{
    class SimpleServer : IServer
    {
        private readonly IConnectionDispatcher _connectionDispatcher;
        private readonly IEnumerable<INetworkAcceptor> _networkAcceptors;

        public SimpleServer(
            IConnectionDispatcher connectionDispatcher,
            IEnumerable<INetworkAcceptor> networkAcceptors)
        {
            _connectionDispatcher = connectionDispatcher;
            _networkAcceptors = networkAcceptors;
        }

        public void StartHost()
        {
            foreach (var acceptor in _networkAcceptors)
            {
                acceptor.Open();
            }

            _connectionDispatcher.StartListen();
        }

        public void StopHost()
        {
            foreach (var acceptor in _networkAcceptors)
            {
                acceptor.Close();
            }

            _connectionDispatcher.StopListen();
        }
    }
}
