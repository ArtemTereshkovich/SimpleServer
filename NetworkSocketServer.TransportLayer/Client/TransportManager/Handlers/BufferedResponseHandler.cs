using System.Threading.Tasks;
using NetworkSocketServer.TransportLayer.Buffer;
using NetworkSocketServer.TransportLayer.Client.Logger;
using NetworkSocketServer.TransportLayer.Client.ServiceHandlers.RequestExecutor.BytesSender;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager.Handlers
{
    internal class BufferedResponseHandler
    {
        private readonly int _packetSizeThreshold;
        private readonly IPacketFactory _packetFactory;
        private readonly IByteSerializer _byteSerializer;
        private readonly IBytesSender _bytesSender;
        private readonly IClientLogger _clientLogger;

        public BufferedResponseHandler(
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
        
        public async Task<byte[]> GetResponseFromServerBuffer(int bufferSize, int increaseStep)
        {
            using var receiveBuffer = InitializeBuffer(bufferSize);

            int totalReceived = 0;

            int portionSize = _packetSizeThreshold;

            while (totalReceived < receiveBuffer.Length)
            {
                portionSize = GetPortionSize(receiveBuffer, totalReceived, portionSize);

                var requestPacket = _packetFactory.CreateRead(totalReceived, portionSize);

                var dataPacket = await SendPacket(requestPacket);

                receiveBuffer.Insert(dataPacket.Payload, totalReceived);

                totalReceived += portionSize;
                portionSize += increaseStep;

                _clientLogger.LogProcessingBytes(totalReceived, receiveBuffer.Length);
            }

            return receiveBuffer.GetAll();
        }

        private static int GetPortionSize(IBuffer buffer, int totalReceived, int portionSize)
        {
            return totalReceived + portionSize > buffer.Length
                ? buffer.Length - totalReceived
                : portionSize;
        }

        private static IBuffer InitializeBuffer(int responseLength)
        {
            var buffer = new ArrayBuffer();
            buffer.Reinitialize(responseLength);

            return buffer;
        }

        private async Task<Packet> SendPacket(Packet sendPacket)
        {
            var dataSerializedPacket = _byteSerializer.Serialize(sendPacket);

            var dataReceived = await _bytesSender.AcceptedSend(dataSerializedPacket);

            return _byteSerializer.Deserialize(dataReceived);
        }
    }
}
