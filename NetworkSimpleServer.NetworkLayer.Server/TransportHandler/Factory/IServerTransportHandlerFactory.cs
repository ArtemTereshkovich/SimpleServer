using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSimpleServer.NetworkLayer.Server.Acceptors;
using NetworkSocketServer.NetworkLayer.Core.TransportHandler;

namespace NetworkSimpleServer.NetworkLayer.Server.TransportHandler.Factory
{
    interface IServerTransportHandlerFactory
    {
        ITransportHandler CreateTransportHandler();
    }
}
