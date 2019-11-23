namespace NetworkSocketServer.Network.TransportHandler.Factories
{
    class SegmentsReceiveTransportHandlerWithRetryFactory : ITransportHandlerFactory
    {
        private readonly int _receiveSegmentSize;
        private readonly TransportHandlerRetryOptions _retryOptions;

        public SegmentsReceiveTransportHandlerWithRetryFactory(int receiveSegmentSize, TransportHandlerRetryOptions retryOptions)
        {
            _receiveSegmentSize = receiveSegmentSize;
            _retryOptions = retryOptions;
        }

        public ITransportHandler CreateTransportHandler()
        {
            var transportHandler = new SegmentsReceiveTransportHandler(_receiveSegmentSize);

            return new TransportHandlerWithRetry(_retryOptions, transportHandler);
        }
    }
}
