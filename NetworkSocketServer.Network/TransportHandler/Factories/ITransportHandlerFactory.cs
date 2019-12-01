using NetworkSocketServer.NetworkLayer.Acceptors;

namespace NetworkSocketServer.NetworkLayer.TransportHandler.Factories
{
    interface ITransportHandlerFactory
    {
        ITransportHandler CreateTransportHandler(INetworkAcceptor acceptor);
    }
}
