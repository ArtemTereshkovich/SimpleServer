namespace NetworkSocketServer.NetworkLayer.TransportHandler.Factories
{
    interface ITransportHandlerFactory
    {
        ITransportHandler CreateTransportHandler();
    }
}
