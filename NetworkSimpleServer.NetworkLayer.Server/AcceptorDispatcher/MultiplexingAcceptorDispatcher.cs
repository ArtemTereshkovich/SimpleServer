using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Server.Acceptors;
using NetworkSimpleServer.NetworkLayer.Server.TransportHandler.Factory;

namespace NetworkSimpleServer.NetworkLayer.Server.AcceptorDispatcher
{
    internal class MultiplexingAcceptorDispatcher : IAcceptorDispatcher
    {
        private readonly ILogger _logger;
        private readonly IServiceConnectionManager _serviceConnectionManager;
        private readonly IServerTransportHandlerFactory _serverTransportHandlerFactory;
        private readonly IList<INetworkAcceptor> _acceptors;

        public MultiplexingAcceptorDispatcher(
            ILogger logger,
            IServiceConnectionManager serviceConnectionManager,
            IServerTransportHandlerFactory serverTransportHandlerFactory)
        {
            _logger = logger;
            _serviceConnectionManager = serviceConnectionManager;
            _serverTransportHandlerFactory = serverTransportHandlerFactory;
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

                    _logger.LogConnectEvent();

                    try
                    {
                        using var transportHandler = _serverTransportHandlerFactory.CreateTransportHandler(acceptor);

                        await acceptor.AcceptConnection(transportHandler);

                        await _serviceConnectionManager.HandleNewConnection(transportHandler);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogErrorException(exception);
                    }
                }

                await _serviceConnectionManager.ProcessRegistered();
            }
        }

        public void StopListen()
        {
            throw new System.NotImplementedException();
        }
    }
}
