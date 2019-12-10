namespace NetworkSocketServer.NetworkLayer.Connectors.Factory
{
    internal interface INetworkConnectorFactory
    {
        INetworkConnector CreateNetworkConnector(NetworkConnectorSettings settings);
    }
}
