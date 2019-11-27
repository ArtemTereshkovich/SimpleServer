using NetworkSocketServer.TransportLayer.Buffer;

namespace NetworkSocketServer.TransportLayer
{
    public class SessionContext
    {
        public int PacketPayloadThreshold { get; } = 1024;

        public IBuffer ReceiveBuffer { get; }

        public IBuffer TransmitBuffer { get; }

        protected SessionContext(IBuffer receiveBuffer, IBuffer transmitBuffer)
        {
            ReceiveBuffer = receiveBuffer;
            TransmitBuffer = transmitBuffer;

            ReceiveBuffer.SetLength(0);
            TransmitBuffer.SetLength(0);
        }

        public static SessionContext CreateNewMemoryStreamBufferContext()
        {
            return new SessionContext(
                new ArrayBuffer(),
                new ArrayBuffer());
        }
    }
}
