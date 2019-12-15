using System.Collections.Generic;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.SocketOptionsAccessor.KeepAlive;
using NetworkSimpleServer.NetworkLayer.Server;
using NetworkSimpleServer.NetworkLayer.Server.Acceptors;
using NetworkSimpleServer.NetworkLayer.Server.TransportHandler.Factory;
using NetworkSocketServer.NetworkLayer.Server.AcceptorDispatcher;
using NetworkSocketServer.NetworkLayer.Server.Acceptors.Tcp;
using NetworkSocketServer.NetworkLayer.Server.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.Server.ServerBuilder
{
    public class SimpleServerBuilder
    {
        private readonly IServiceConnectionManagerFactory _serviceConnectionManagerFactory;
        private readonly IServerTransportHandlerFactory _transportHandlerFactory;
        private readonly IList<INetworkAcceptor> _acceptors;

        public SimpleServerBuilder(IServiceConnectionManagerFactory serviceConnectionManagerFactory)
        {
            _serviceConnectionManagerFactory = serviceConnectionManagerFactory;
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
        
        public IServer Build()
        {
            var dispatcher = new MultiThreadAcceptorDispatcher(
                _transportHandlerFactory,
                new ConsoleLogger(), 
                _serviceConnectionManagerFactory);
            
            foreach (var acceptor in _acceptors)
            {
                dispatcher.RegisterAcceptor(acceptor);   
            }

            return new SimpleServer(dispatcher, _acceptors);
        }
    }
}
