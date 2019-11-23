using System.Collections.Generic;
using System.Threading;
using NetworkSocketServer.Network.ThreadSet;
using NetworkSocketServer.Network.TransportHandler;

namespace NetworkSocketServer.Network.ConnectionDispatcher
{
    internal class ThreadSetAcceptorDispatcher : IConnectionDispatcher
    {
        private readonly IThreadSet _threadSet;
        private readonly INetworkServiceHandler _serviceHandler;
        private readonly ITransportHandlerFactory _transportHandlerFactory;
        private readonly IList<INetworkAcceptor> _acceptors;
        private readonly Thread _workThread;

        public ThreadSetAcceptorDispatcher(
            IThreadSet threadSet,
            INetworkServiceHandler serviceHandler,
            ITransportHandlerFactory transportHandlerFactory)
        {
            _threadSet = threadSet;
            _serviceHandler = serviceHandler;
            _transportHandlerFactory = transportHandlerFactory;
            _workThread = new Thread(InternalStart);
            _acceptors = new List<INetworkAcceptor>();
        }

        public void RegisterAcceptor(INetworkAcceptor acceptor)
        {
            _acceptors.Add(acceptor);
        }

        public void StartListen()
        {
            _workThread.Start();
        }

        private void InternalStart()
        {
            while (true)
            {
                foreach (var acceptor in _acceptors)
                {
                    if (!acceptor.IsHaveNewConnection()) continue;

                    _threadSet.Execute(async networkAcceptor =>
                    {
                        var transportHandler = _transportHandlerFactory.CreateTransportHandler();
                        await networkAcceptor.AcceptConnection(transportHandler);

                        await _serviceHandler.HandleNewConnection(transportHandler);

                    }, acceptor);
                }
            }
        }

        public void StopListen()
        {
            _workThread.Abort();
        }
    }
}