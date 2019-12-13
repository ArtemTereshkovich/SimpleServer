namespace NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler.Factory
{
    public interface IClientTransportHandlerFactory
    {
        IClientTransportHandler CreateTransportHandler();
    }
}
