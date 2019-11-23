namespace NetworkSocketServer.Network.TransportHandler.Factories
{
    internal class DirectTransportHandlerWithRetryFactory : ITransportHandlerFactory
    {
        private readonly TransportHandlerRetryOptions _retryOptions;

        public DirectTransportHandlerWithRetryFactory(TransportHandlerRetryOptions retryOptions)
        {
            _retryOptions = retryOptions;
        }

        public ITransportHandler CreateTransportHandler()
        {
            var transportHandler = new DirectTransportHandler();

            return  new TransportHandlerWithRetry(_retryOptions, transportHandler);
        }
    }
}
