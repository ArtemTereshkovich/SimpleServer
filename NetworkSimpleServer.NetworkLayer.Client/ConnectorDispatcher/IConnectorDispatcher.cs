using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;
using NetworkSimpleServer.NetworkLayer.Client.Connectors;

namespace NetworkSimpleServer.NetworkLayer.Client.ConnectorDispatcher
{
    public interface IConnectorDispatcher
    {
        IClientTransportHandler CreateTransportHandler(NetworkConnectorSettings settings);
    }
}
