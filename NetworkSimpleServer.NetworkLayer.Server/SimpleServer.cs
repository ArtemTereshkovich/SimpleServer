using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Server.Acceptors;
using NetworkSocketServer.NetworkLayer.Server.AcceptorDispatcher;

namespace NetworkSocketServer.NetworkLayer.Server
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

        public async Task StartHost()
        {
            foreach (var acceptor in _networkAcceptors)
            {
                acceptor.Open();
            }

            await _acceptorDispatcher.StartListen();
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