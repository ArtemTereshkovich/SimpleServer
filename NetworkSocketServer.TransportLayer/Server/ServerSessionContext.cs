using NetworkSocketServer.TransportLayer.Buffer;

namespace NetworkSocketServer.TransportLayer.Server
{
    public class ServerSessionContext
    {
        public int PacketPayloadThreshold { get; } = 1024;

        public IBuffer ReceiveBuffer { get; }

        public IBuffer TransmitBuffer { get; }

        private ServerSessionContext(IBuffer receiveBuffer, IBuffer transmitBuffer)
        {
            ReceiveBuffer = receiveBuffer;
            TransmitBuffer = transmitBuffer;

            ReceiveBuffer.Reinitialize(0);
            TransmitBuffer.Reinitialize(0);
        }

        public static ServerSessionContext CreateArrayBuffersContext()
        {
            return new ServerSessionContext(
                new ArrayBuffer(),
                new ArrayBuffer());
        }
    }
}
