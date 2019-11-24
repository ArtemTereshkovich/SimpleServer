namespace NetworkSocketServer.NetworkLayer.TransportHandler.Factories
{
    internal class BlockingTransportHandlerFactory : ITransportHandlerFactory
    {
        public ITransportHandler CreateTransportHandler()
        {
            return new BlockingReceiveTransportHandler();
        }
    }
}
