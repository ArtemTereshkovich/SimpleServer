using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Server.Acceptors;
using NetworkSimpleServer.NetworkLayer.Server.TransportHandler.Factory;

namespace NetworkSocketServer.NetworkLayer.Server.AcceptorDispatcher
{
    class MultiThreadAcceptorDispatcher : IAcceptorDispatcher
    {
        private readonly ILogger _logger;
        private readonly IServiceConnectionManagerFactory _serviceConnectionManagerFactory;
        private readonly IServerTransportHandlerFactory _serverTransportHandlerFactory;
        private readonly IList<INetworkAcceptor> _acceptors;
        private readonly IList<Thread> _threads;

        public MultiThreadAcceptorDispatcher(
            IServerTransportHandlerFactory serverTransportHandlerFactory, 
            ILogger logger, 
            IServiceConnectionManagerFactory serviceConnectionManagerFactory)
        {
            _serverTransportHandlerFactory = serverTransportHandlerFactory;
            _logger = logger;
            _serviceConnectionManagerFactory = serviceConnectionManagerFactory;
            _acceptors = new List<INetworkAcceptor>();
            _threads = new List<Thread>();
        }

        public void RegisterAcceptor(INetworkAcceptor acceptor)
        {
            _acceptors.Add(acceptor);
        }

        public void StartListen()
        {
            while (true)
            {
                foreach (var acceptor in _acceptors)
                {
                    if (!acceptor.IsHaveNewConnection()) continue;

                    _logger.LogConnectEvent();

                    var thread = new Thread(() => 
                        HandleNewConnection(acceptor, _serviceConnectionManagerFactory)
                            .Wait());

                    _threads.Add(thread);

                    thread.Start();
                }
            }
        }

        private async Task HandleNewConnection(
            INetworkAcceptor acceptor,
            IServiceConnectionManagerFactory factory)
        {
            var transportHandler = _serverTransportHandlerFactory.CreateTransportHandler();

            await acceptor.AcceptConnection(transportHandler);

            var connectionManager =  factory.CreateConnectionManager();

            await connectionManager.HandleNewConnection(transportHandler);
        }

        public void StopListen()
        {
            throw new NotImplementedException();
        }
    }
}
