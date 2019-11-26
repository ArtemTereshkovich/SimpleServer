using NetworkSocketServer.TransportLayer.Buffer;

namespace NetworkSocketServer.TransportLayer
{
    internal class SessionContext
    {
        public int PacketPayloadThreshold { get; } = 1024;

        public IBuffer ReceiveBuffer { get; }

        public IBuffer TransmitBuffer { get; }

        protected SessionContext(IBuffer receiveBuffer, IBuffer transmitBuffer)
        {
            ReceiveBuffer = receiveBuffer;
            TransmitBuffer = transmitBuffer;
        }

        public static SessionContext CreateNewMemoryStreamBufferContext()
        {
            return new SessionContext(
                new MemoryStreamBuffer(),
                new MemoryStreamBuffer());
        }
    }
}
