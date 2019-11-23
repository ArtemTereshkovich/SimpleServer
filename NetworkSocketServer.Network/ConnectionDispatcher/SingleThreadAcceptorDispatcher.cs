using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkSocketServer.Network.TransportHandler;

namespace NetworkSocketServer.Network.ConnectionDispatcher
{
    internal class SingleThreadAcceptorDispatcher : IConnectionDispatcher
    {
        private readonly INetworkServiceHandler _serviceHandler;
        private readonly ITransportHandlerFactory _transportHandlerFactory;
        private readonly IList<INetworkAcceptor> _acceptors;

        public SingleThreadAcceptorDispatcher(
            INetworkServiceHandler serviceHandler,
            ITransportHandlerFactory transportHandlerFactory)
        {
            _serviceHandler = serviceHandler;
            _transportHandlerFactory = transportHandlerFactory;
            _acceptors = new List<INetworkAcceptor>();
        }

        public void RegisterAcceptor(INetworkAcceptor acceptor)
        {
            _acceptors.Add(acceptor);
        }

        public void StartListen()
        {
            InternalStart().Wait();
        }

        private async Task InternalStart()
        {
            while (true)
            {
                foreach (var acceptor in _acceptors)
                {
                    if (!acceptor.IsHaveNewConnection()) continue;

                        var transportHandler = _transportHandlerFactory.CreateTransportHandler();
                        await acceptor.AcceptConnection(transportHandler);

                        await _serviceHandler.HandleNewConnection(transportHandler);
                }
            }
        }

        public void StopListen()
        {
            throw new NotSupportedException();
        }
    }
}
