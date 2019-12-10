using System.Collections.Generic;
using NetworkSimpleServer.NetworkLayer.Core.SocketOptionsAccessor.KeepAlive;
using NetworkSimpleServer.NetworkLayer.Server.AcceptorDispatcher;
using NetworkSimpleServer.NetworkLayer.Server.Acceptors;
using NetworkSimpleServer.NetworkLayer.Server.Acceptors.Tcp;
using NetworkSimpleServer.NetworkLayer.Server.Acceptors.Udp;
using NetworkSimpleServer.NetworkLayer.Server.TransportHandler;
using NetworkSimpleServer.NetworkLayer.Server.TransportHandler.Factory;

namespace NetworkSimpleServer.NetworkLayer.Server.ServerBuilder
{
    public class SimpleServerBuilder
    {
        private readonly IServiceConnectionManager _serviceConnectionManager;
        private readonly IServerTransportHandlerFactory _transportHandlerFactory;
        private readonly IList<INetworkAcceptor> _acceptors;

        public SimpleServerBuilder(IServiceConnectionManager serviceConnectionManager)
        {
            _serviceConnectionManager = serviceConnectionManager;
            _transportHandlerFactory = new SimpleBlockingTransportHandlerFactory();
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

        public IServer Build()
        {
            var dispatcher = new SingleThreadAcceptorDispatcher(_serviceConnectionManager, _transportHandlerFactory);
            
            foreach (var acceptor in _acceptors)
            {
                dispatcher.RegisterAcceptor(acceptor);   
            }

            return new SimpleServer(dispatcher, _acceptors);
        }
    }
}
