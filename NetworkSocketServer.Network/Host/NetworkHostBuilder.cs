using System.Collections.Generic;
using NetworkSocketServer.Network.ConnectionDispatcher;
using NetworkSocketServer.Network.Tcp;
using NetworkSocketServer.Network.Tcp.KeepAlive;
using NetworkSocketServer.Network.ThreadSet;
using NetworkSocketServer.Network.TransportHandler;
using NetworkSocketServer.Network.TransportHandler.Factories;

namespace NetworkSocketServer.Network.Host
{
    public class NetworkHostBuilder
    {
        private readonly INetworkServiceHandler _networkServiceHandler;
        
        private readonly IList<INetworkAcceptor> _acceptors;

        private int? _maxThreadsNumber = null;
        private ITransportHandlerFactory _transportHandlerFactory;

        public NetworkHostBuilder(INetworkServiceHandler networkServiceHandler)
        {
            _networkServiceHandler = networkServiceHandler;
            _transportHandlerFactory = new DirectTransportHandlerFactory();
            _acceptors = new List<INetworkAcceptor>();
        }

        public NetworkHostBuilder WithTcpFaultToleranceAcceptor(
            TcpNetworkAcceptorSettings acceptorSettings,
            SocketFaultToleranceOptions socketFaultToleranceOptions)
        {
            var socketOptionsAccessor = new PlatformBasedSocketOptionsAccessorFactory(socketFaultToleranceOptions);

            _acceptors.Add(new TcpKeepAliveNetworkAcceptor(acceptorSettings, socketOptionsAccessor));

            return this;
        }

        public NetworkHostBuilder WithMultiThreadPoolAcceptorDispatcher(int maxThreadsNumber)
        {
            _maxThreadsNumber = maxThreadsNumber;

            return this;
        }

        public NetworkHostBuilder WithSegmentsRetryTransportHandler(int receiveSegmentSize, TransportHandlerRetryOptions retryOptions)
        {
            _transportHandlerFactory = new SegmentsReceiveTransportHandlerWithRetryFactory(receiveSegmentSize, retryOptions);

            return this;
        }

        public NetworkHostBuilder WithDirectRetryTransportHandler(TransportHandlerRetryOptions retryOptions)
        {
            _transportHandlerFactory = new DirectTransportHandlerWithRetryFactory(retryOptions);

            return this;
        }

        public INetworkHost Build()
        {
            IConnectionDispatcher dispatcher = null;

            if (_maxThreadsNumber != null)
            {
                dispatcher = new ThreadSetAcceptorDispatcher(new ThreadPoolThreadSet(_maxThreadsNumber.Value),
                    _networkServiceHandler, _transportHandlerFactory);
            }
            else
            {
                dispatcher = new SingleThreadAcceptorDispatcher(_networkServiceHandler, _transportHandlerFactory);
            }


            foreach (var acceptor in _acceptors)
            {
                dispatcher.RegisterAcceptor(acceptor);   
            }

            return new NetworkHost(dispatcher, _acceptors);
        }
    }
}
