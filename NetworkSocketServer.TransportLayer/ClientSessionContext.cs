using System;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.Buffer;

namespace NetworkSocketServer.TransportLayer
{
    internal class ClientSessionContext : SessionContext
    {
        public ITransportHandler TransportHandler { get; }

        public Guid SessionId { get; }

        protected ClientSessionContext(ITransportHandler transportHandler, IBuffer receiveBuffer,
            IBuffer transmitBuffer, Guid sessionId) : base(receiveBuffer, transmitBuffer)
        {

        }

        public static ClientSessionContext CreateClientContext(ITransportHandler transportHandler)
        {
            return new ClientSessionContext(
                transportHandler,
                new MemoryStreamBuffer(),
                new MemoryStreamBuffer(),
                Guid.NewGuid());
        }
    }
}
