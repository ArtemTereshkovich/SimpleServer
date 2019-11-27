using NetworkSocketServer.TransportLayer.ServiceHandlers.NetworkClientManager;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor
{
    public class RequestExecutorFactory : IRequestExecutorFactory
    {
        private readonly RetrySettings _retrySettings;

        public RequestExecutorFactory(RetrySettings retrySettings)
        {
            _retrySettings = retrySettings;
        }

        public IRequestExecutor Create(NetworkClientManager.NetworkClientManager networkClientManager)
        {
           return new RequestExecutor(networkClientManager, _retrySettings);
        }
    }
}
