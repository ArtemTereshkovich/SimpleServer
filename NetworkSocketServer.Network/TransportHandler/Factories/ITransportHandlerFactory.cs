namespace NetworkSocketServer.Network.TransportHandler
{
    interface ITransportHandlerFactory
    {
        ITransportHandler CreateTransportHandler();
    }
}
