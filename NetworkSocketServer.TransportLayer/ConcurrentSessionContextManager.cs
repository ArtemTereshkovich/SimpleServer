using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.Buffer;
using NetworkSocketServer.TransportLayer.Messages;
using NetworkSocketServer.TransportLayer.Serializer;

namespace NetworkSocketServer.TransportLayer
{
    public class ConcurrentSessionContextManager : INewTransportHandler, ISessionContextManager
    {
        private readonly IDictionary<Guid, SessionContext> _sessionContexts;
        private readonly IByteSerializer _byteSerializer;

        public ConcurrentSessionContextManager()
        {
            _byteSerializer = new BinaryFormatterByteSerializer();
            _sessionContexts = new ConcurrentDictionary<Guid, SessionContext>();
        }

        public async Task HandleNewConnection(ITransportHandler transportHandler)
        {
            var welcomeMessage = await ReceiveMessage(transportHandler);

            if (_sessionContexts.TryGetValue(welcomeMessage.ClientId, out var sessionContext))
            {

            }
            else
            {
                var initializeContext = InitializeContext(
                    welcomeMessage.ClientId,
                    welcomeMessage.MessageOptions);

                _sessionContexts.Add(welcomeMessage.ClientId, initializeContext);
            }
        }

        private async Task<WelcomeMessage> ReceiveMessage(ITransportHandler transportHandler)
        {
            return _byteSerializer.Deserialize<WelcomeMessage>(await transportHandler.ReceiveAllAvailable());
        }

        private static SessionContext InitializeContext(Guid clientId, MessageOptions messageOptions)
        {
            return new SessionContext(
                clientId,
                new MemoryStreamBuffer(),
                new MemoryStreamBuffer(),
                messageOptions);
        }

        public void ClearContext(Guid clientId)
        {
            _sessionContexts.Remove(clientId);
        }
    }
}
