using System.Threading.Tasks;
using NetworkSocketServer.TransportLayer.Buffer;
using NetworkSocketServer.TransportLayer.Client.Logger;
using NetworkSocketServer.TransportLayer.Client.ServiceHandlers.RequestExecutor.BytesSender;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager.Handlers
{
    class BufferedRequestHandler
    {
        private readonly int _packetSizeThreshold;
        private readonly IPacketFactory _packetFactory;
        private readonly IByteSerializer _byteSerializer;
        private readonly IBytesSender _bytesSender;
        private readonly IClientLogger _clientLogger;

        public BufferedRequestHandler(
            int packetSizeThreshold, 
            IPacketFactory packetFactory, 
            IByteSerializer byteSerializer, 
            IBytesSender bytesSender, 
            IClientLogger clientLogger)
        {
            _packetSizeThreshold = packetSizeThreshold;
            _packetFactory = packetFactory;
            _byteSerializer = byteSerializer;
            _bytesSender = bytesSender;
            _clientLogger = clientLogger;
        }

        public async Task<Packet> ProvideRequestToServerBuffer(byte[] request, int increaseStep)
        {
            using var sendBuffer = InitializeBuffer(request);
            
            int totalSend = 0;

            int portionSize = _packetSizeThreshold;

            while (totalSend < sendBuffer.Length)
            {
                portionSize = GetPortionSize(sendBuffer, totalSend, portionSize);
                
                var bytesToSend = sendBuffer.Get(totalSend, portionSize);

                var dataPacket = _packetFactory.CreateWrite(
                    bytesToSend, 
                    sendBuffer.Length, 
                    totalSend);

                await SendPacket(dataPacket);

                totalSend += portionSize;
                portionSize += increaseStep;

                _clientLogger.LogProcessingBytes(totalSend, sendBuffer.Length);
            }

            return _packetFactory.CreateExecuteBuffer(sendBuffer.Length);
        }

        private async Task SendPacket(Packet dataPacket)
        {
            var dataSerializedPacket = _byteSerializer.Serialize(dataPacket);

            await _bytesSender.AcceptedSend(dataSerializedPacket);
        }

        private static int GetPortionSize(IBuffer buffer, int totalSend, int portionSize)
        {
            return totalSend + portionSize > buffer.Length
                ? buffer.Length - totalSend
                : portionSize;
        }

        private static IBuffer InitializeBuffer(byte[] request)
        {
            var buffer = new ArrayBuffer();
            buffer.Reinitialize(request.Length);
            buffer.Insert(request, 0);

            return buffer;
        }
    }
}
