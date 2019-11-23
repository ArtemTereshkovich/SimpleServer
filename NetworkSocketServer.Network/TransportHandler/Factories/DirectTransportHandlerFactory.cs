namespace NetworkSocketServer.Network.TransportHandler
{
    internal class DirectTransportHandlerFactory : ITransportHandlerFactory
    {
        public ITransportHandler CreateTransportHandler()
        {
            return new DirectTransportHandler();
        }
    }
}
