using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.Acceptors;
using NetworkSocketServer.NetworkLayer.TransportHandler.Factories;

namespace NetworkSocketServer.NetworkLayer.Dispatchers.AcceptorDispatcher
{
    internal class SingleThreadAcceptorDispatcher : IAcceptorDispatcher
    {
        private readonly IConnectionManager _connectionManager;
        private readonly ITransportHandlerFactory _transportHandlerFactory;
        private readonly IList<INetworkAcceptor> _acceptors;

        public SingleThreadAcceptorDispatcher(
            IConnectionManager connectionManager,
            ITransportHandlerFactory transportHandlerFactory)
        {
            _connectionManager = connectionManager;
            _transportHandlerFactory = transportHandlerFactory;
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

                    Console.WriteLine("Receive new connection");
                    try
                    {
                        using var transportHandler = _transportHandlerFactory.CreateTransportHandler(acceptor);

                        await acceptor.AcceptConnection(transportHandler);

                        await _connectionManager.HandleNewConnection(transportHandler);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Error happend:" + exception.Message);
                    }
                }
            }
        }

        public void StopListen()
        {
            throw new NotSupportedException();
        }
    }
}
