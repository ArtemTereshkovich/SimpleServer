using NetworkSocketServer.NetworkLayer.Acceptors;
using NetworkSocketServer.NetworkLayer.Connectors;

namespace NetworkSocketServer.NetworkLayer.TransportHandler.Factories
{
    interface ITransportHandlerFactory
    {
        ITransportHandler CreateTransportHandler(INetworkAcceptor acceptor);

        ITransportHandler CreateTransportHandler(ConnectionType connwctionType);
    }
}
