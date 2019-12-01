using System.Collections.Generic;
using NetworkSocketServer.NetworkLayer.Acceptors;
using NetworkSocketServer.NetworkLayer.Acceptors.Tcp;
using NetworkSocketServer.NetworkLayer.Acceptors.Udp;
using NetworkSocketServer.NetworkLayer.Dispatchers.AcceptorDispatcher;
using NetworkSocketServer.NetworkLayer.Server;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;
using NetworkSocketServer.NetworkLayer.ThreadSet;
using NetworkSocketServer.NetworkLayer.TransportHandler.Factories;

namespace NetworkSocketServer.NetworkLayer.ServerBuilder
{
    public class SimpleServerBuilder
    {
        private readonly IConnectionManager _connectionManager;
        private readonly ITransportHandlerFactory _transportHandlerFactory;
        private readonly IList<INetworkAcceptor> _acceptors;

        private int? _maxThreadsNumber = null;

        public SimpleServerBuilder(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
            _transportHandlerFactory = new BlockingTransportHandlerFactory();
            _acceptors = new List<INetworkAcceptor>();
        }

        public SimpleServerBuilder WithTcpKeepAliveAcceptor(
            TcpNetworkAcceptorSettings acceptorSettings,
            SocketKeepAliveOptions socketKeepAliveOptions)
        {
            var socketOptionsAccessor = new PlatformBasedKeepAliveAccessorFactory(socketKeepAliveOptions);

            _acceptors.Add(new TcpKeepAliveNetworkAcceptor(acceptorSettings, socketOptionsAccessor));

            return this;
        }

        public SimpleServerBuilder WithUdpAcceptor(UdpNetworkAcceptorSettings acceptorSettings)
        {
            _acceptors.Add(new UdpNetworkAcceptor(acceptorSettings));

            return this;
        }

        public SimpleServerBuilder WithMultiThreadPoolAcceptorDispatcher(int maxThreadsNumber)
        {
            _maxThreadsNumber = maxThreadsNumber;

            return this;
        }

        public IServer Build()
        {
            IAcceptorDispatcher dispatcher = null;

            if (_maxThreadsNumber != null)
            {
                dispatcher = new ThreadSetAcceptorDispatcher(new ThreadPoolThreadSet(_maxThreadsNumber.Value),
                    _connectionManager, _transportHandlerFactory);
            }
            else
            {
                dispatcher = new SingleThreadAcceptorDispatcher(_connectionManager, _transportHandlerFactory);
            }


            foreach (var acceptor in _acceptors)
            {
                dispatcher.RegisterAcceptor(acceptor);   
            }

            return new SimpleServer(dispatcher, _acceptors);
        }
    }
}
