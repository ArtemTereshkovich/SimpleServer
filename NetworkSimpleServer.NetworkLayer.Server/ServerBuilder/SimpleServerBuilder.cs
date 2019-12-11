using System.Collections.Generic;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
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
            var acceptors = new List<INetworkAcceptor>();

            acceptors.Add(new UdpNetworkAcceptor(acceptorSettings));

            CheckDuplicate(acceptorSettings);

            return this;
        }

        public IServer Build()
        {
            var dispatcher = new SingleThreadAcceptorDispatcher(new ConsoleLogger(), _serviceConnectionManager, _transportHandlerFactory);
            
            foreach (var acceptor in _acceptors)
            {
                dispatcher.RegisterAcceptor(acceptor);   
            }

            return new SimpleServer(dispatcher, _acceptors);
        }



        #region privateMethods
        private void CheckDuplicate(UdpNetworkAcceptorSettings settings)
        {
            var tcpSetting = new TcpNetworkAcceptorSettings
            {
                ListenIpAddress = settings.ListenIpAddress,
                ListenMaxBacklogConnection = 1,
                ListenPort = settings.ListenPort,
            };
            var keepAliev = new SocketKeepAliveOptions
            {
                KeepAliveInterval = 30000,
                KeepAliveTime = 30000,
            };

            var socketOptionsAccessor = new PlatformBasedKeepAliveAccessorFactory(keepAliev);

            _acceptors.Add(new TcpKeepAliveNetworkAcceptor(tcpSetting, socketOptionsAccessor));

        }
        #endregion
    }
}
