using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;

namespace NetworkSimpleServer.NetworkLayer.Client.Connectors
{
    internal interface INetworkConnector
    {
        void Activate(IClientTransportHandler transportHandler);
    }
}
