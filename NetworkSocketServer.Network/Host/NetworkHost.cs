using System.Collections.Generic;
using NetworkSocketServer.Network.ConnectionDispatcher;

namespace NetworkSocketServer.Network.Host
{
    class NetworkHost : INetworkHost
    {
        private readonly IConnectionDispatcher _connectionDispatcher;
        private readonly IEnumerable<INetworkAcceptor> _networkAcceptors;

        public NetworkHost(IConnectionDispatcher connectionDispatcher, IEnumerable<INetworkAcceptor>  networkAcceptors)
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
