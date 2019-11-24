using System.Collections.Generic;
using NetworkSocketServer.NetworkLayer.Acceptors;
using NetworkSocketServer.NetworkLayer.Acceptors.Tcp;
using NetworkSocketServer.NetworkLayer.ConnectionDispatcher;
using NetworkSocketServer.NetworkLayer.Server;
using NetworkSocketServer.NetworkLayer.Tcp.KeepAlive;
using NetworkSocketServer.NetworkLayer.ThreadSet;
using NetworkSocketServer.NetworkLayer.TransportHandler.Factories;

namespace NetworkSocketServer.NetworkLayer.ServerBuilder
{
    public class SimpleServerBuilder
    {
        private readonly INewTransportHandler _newTransportHandler;
        private readonly ITransportHandlerFactory _transportHandlerFactory;
        private readonly IList<INetworkAcceptor> _acceptors;

        private int? _maxThreadsNumber = null;

        public SimpleServerBuilder(INewTransportHandler newTransportHandler)
        {
            _newTransportHandler = newTransportHandler;
            _transportHandlerFactory = new BlockingTransportHandlerFactory();
            _acceptors = new List<INetworkAcceptor>();
        }

        public SimpleServerBuilder WithTcpFaultToleranceAcceptor(
            TcpNetworkAcceptorSettings acceptorSettings,
            SocketFaultToleranceOptions socketFaultToleranceOptions)
        {
            var socketOptionsAccessor = new PlatformBasedSocketOptionsAccessorFactory(socketFaultToleranceOptions);

            _acceptors.Add(new TcpKeepAliveNetworkAcceptor(acceptorSettings, socketOptionsAccessor));

            return this;
        }

        public SimpleServerBuilder WithMultiThreadPoolAcceptorDispatcher(int maxThreadsNumber)
        {
            _maxThreadsNumber = maxThreadsNumber;

            return this;
        }

        public IServer Build()
        {
            IConnectionDispatcher dispatcher = null;

            if (_maxThreadsNumber != null)
            {
                dispatcher = new ThreadSetAcceptorDispatcher(new ThreadPoolThreadSet(_maxThreadsNumber.Value),
                    _newTransportHandler, _transportHandlerFactory);
            }
            else
            {
                dispatcher = new SingleThreadAcceptorDispatcher(_newTransportHandler, _transportHandlerFactory);
            }


            foreach (var acceptor in _acceptors)
            {
                dispatcher.RegisterAcceptor(acceptor);   
            }

            return new SimpleServer(dispatcher, _acceptors);
        }
    }
}
