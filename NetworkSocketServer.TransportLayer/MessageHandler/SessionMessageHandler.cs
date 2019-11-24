using System;
using System.Threading.Tasks;

namespace NetworkSocketServer.TransportLayer.MessageHandler
{
    internal class SessionMessageHandler : IMessageHandler
    {
        private readonly ISessionContextManager _sessionContextManager;
        private readonly SessionContext _sessionContext;

        public Guid ClientId => _sessionContext.ClientId;

        public SessionMessageHandler(
            ISessionContextManager sessionContextManager,
            SessionContext sessionContext)
        {
            _sessionContextManager = sessionContextManager;
            _sessionContext = sessionContext;
        }

        Task<>
       

        

        public Task Send(byte[] array)
        {
            throw new System.NotImplementedException();
        }

        public Task<byte> ReceiveMessage()
        {
            throw new System.NotImplementedException();
        }

        public void CloseHandler()
        {
            _sessionContextManager.ClearContext(_sessionContext.ClientId);
        }

        public void Dispose()
        {
            CloseHandler();
        }
    }
}
