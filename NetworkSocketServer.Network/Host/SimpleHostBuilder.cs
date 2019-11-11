using System.Collections.Generic;
using NetworkSocketServer.Network.ConnectionDispatcher;
using NetworkSocketServer.Network.Tcp;
using NetworkSocketServer.Network.Tcp.KeepAlive;
using NetworkSocketServer.Network.ThreadSet;
using NetworkSocketServer.Network.TransportHandler;

namespace NetworkSocketServer.Network.Host
{
    public class SimpleHostBuilder
    {
        private readonly INetworkServiceHandler _networkServiceHandler;
        private readonly ITransportHandlerFactory _transportHandlerFactory;
        private readonly int _threadNumbers;
        private readonly IList<INetworkAcceptor> _acceptors;

        public SimpleHostBuilder(INetworkServiceHandler networkServiceHandler, int threadNumbers)
        {
            _networkServiceHandler = networkServiceHandler;
            _threadNumbers = threadNumbers;
            _transportHandlerFactory = new DirectTransportHandlerFactory();
            _acceptors = new List<INetworkAcceptor>();
        }

        public SimpleHostBuilder WithTcpFaultToleranceAcceptor(
            TcpNetworkAcceptorSettings acceptorSettings,
            SocketFaultToleranceOptions socketFaultToleranceOptions)
        {
            var socketOptionsAccessor = new PlatformBasedSocketOptionsAccessorFactory(socketFaultToleranceOptions);

            _acceptors.Add(new TcpKeepAliveNetworkAcceptor(acceptorSettings, socketOptionsAccessor));

            return this;
        }

        public ISimpleHost Build()
        {
            IThreadSet threadSet = new StubThreadSet();

            var dispatcher = new MultiThreadNetworkAcceptorDispatcher(
                threadSet, 
                _networkServiceHandler, 
                _transportHandlerFactory);

            foreach (var acceptor in _acceptors)
            {
                dispatcher.RegisterAcceptor(acceptor);   
            }

            return new SimpleHost(dispatcher, _acceptors);
        }
    }
}
