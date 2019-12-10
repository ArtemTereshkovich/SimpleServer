using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSimpleServer.NetworkLayer.Server.Acceptors;

namespace NetworkSimpleServer.NetworkLayer.Server.TransportHandler.Factory
{
    interface IServerTransportHandlerFactory
    {
        ITransportHandler CreateTransportHandler(INetworkAcceptor acceptor);
    }
}
