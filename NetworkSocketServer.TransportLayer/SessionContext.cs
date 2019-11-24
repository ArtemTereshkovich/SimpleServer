using System;
using NetworkSocketServer.TransportLayer.Buffer;

namespace NetworkSocketServer.TransportLayer
{
    internal class SessionContext : IDisposable
    {
        public Guid ClientId { get; }

        public MessageOptions MessageOptions { get; }

        public IBuffer ReceiveBuffer { get; }

        public IBuffer TransmitBuffer { get; }

        public SessionContext(
            Guid clientId, 
            IBuffer receiveBuffer,
            IBuffer transmitBuffer,
            MessageOptions messageOptions)
        {
            ClientId = clientId;
            ReceiveBuffer = receiveBuffer;
            TransmitBuffer = transmitBuffer;
            MessageOptions = messageOptions;
        }

        public void Dispose()
        {
            ReceiveBuffer?.Dispose();

            TransmitBuffer?.Dispose();
        }
    }
}
