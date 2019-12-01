using System.Collections.Generic;
using System.Threading;
using NetworkSocketServer.NetworkLayer.Acceptors;
using NetworkSocketServer.NetworkLayer.ThreadSet;
using NetworkSocketServer.NetworkLayer.TransportHandler.Factories;

namespace NetworkSocketServer.NetworkLayer.Dispatchers.AcceptorDispatcher
{
    internal class ThreadSetAcceptorDispatcher : IAcceptorDispatcher
    {
        private readonly IThreadSet _threadSet;
        private readonly IConnectionManager _serviceHandler;
        private readonly ITransportHandlerFactory _transportHandlerFactory;
        private readonly IList<INetworkAcceptor> _acceptors;
        private readonly Thread _workThread;

        public ThreadSetAcceptorDispatcher(
            IThreadSet threadSet,
            IConnectionManager serviceHandler,
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
                        using var transportHandler = _transportHandlerFactory.CreateTransportHandler(acceptor);

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