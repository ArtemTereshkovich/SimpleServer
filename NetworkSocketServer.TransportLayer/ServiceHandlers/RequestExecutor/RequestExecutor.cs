using System.Threading.Tasks;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor
{
    class RequestExecutor : IRequestExecutor
    {
        private readonly NetworkClientManager.NetworkClientManager _networkClientManager;
        private readonly RetrySettings _retrySettings;

        public RequestExecutor(
            NetworkClientManager.NetworkClientManager networkClientManager, 
            RetrySettings retrySettings)
        {
            _networkClientManager = networkClientManager;
            _retrySettings = retrySettings;
        }

        public Task<byte[]> HandleRequest(byte[] request)
        {
            throw new System.NotImplementedException();
        }
    }
}
