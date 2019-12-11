using System;
using NetworkSocketServer.TransportLayer.Core.Buffer;

namespace NetworkSocketServer.TransportLayer.Server
{
    public class ServerSessionContext
    {
        public Guid SessionId { get; }

        public IBuffer ReceiveBuffer { get; }

        public IBuffer TransmitBuffer { get; }

        private ServerSessionContext(
            Guid sessionId,
            IBuffer receiveBuffer, 
            IBuffer transmitBuffer)
        {
            SessionId = sessionId;
            ReceiveBuffer = receiveBuffer;
            TransmitBuffer = transmitBuffer;

            ReceiveBuffer.Reinitialize(0);
            TransmitBuffer.Reinitialize(0);
        }

        public static ServerSessionContext CreateArrayBuffersContext(Guid sessionId)
        {
            return new ServerSessionContext(
                sessionId,
                new ArrayBuffer(),
                new ArrayBuffer());
        }
    }
}
