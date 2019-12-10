using NetworkSimpleServer.NetworkLayer.Client.Connectors;

namespace NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler.Factory
{
    public interface IClientTransportHandlerFactory
    {
        IClientTransportHandler CreateTransportHandler(ConnectionType connectionType);
    }
}
