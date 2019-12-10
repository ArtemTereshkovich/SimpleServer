namespace NetworkSimpleServer.NetworkLayer.Client.Connectors.Factory
{
    internal interface INetworkConnectorFactory
    {
        INetworkConnector CreateNetworkConnector(NetworkConnectorSettings settings);
    }
}
