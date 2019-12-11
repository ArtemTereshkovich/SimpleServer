using System.Linq;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSocketServer.TransportLayer.Core.Buffer;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager.Handlers
{
    internal class BufferedResponseHandler
    {
        private readonly int _packetSizeThreshold;
        private readonly IPacketFactory _packetFactory;
        private readonly IByteSerializer _byteSerializer;
        private readonly IBytesSender _bytesSender;
        private readonly ILogger _logger;

        public BufferedResponseHandler(
            int packetSizeThreshold, 
            IPacketFactory packetFactory, 
            IByteSerializer byteSerializer, 
            IBytesSender bytesSender,
            ILogger logger)
        {
            _packetSizeThreshold = packetSizeThreshold;
            _packetFactory = packetFactory;
            _byteSerializer = byteSerializer;
            _bytesSender = bytesSender;
            _logger = logger;
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

                var dataPacket = await SendPacket(requestPacket, portionSize + 36);

                var payload = dataPacket.Payload.Take(dataPacket.PayloadSize).ToArray();

                receiveBuffer.Insert(payload, totalReceived);

                totalReceived += portionSize;

                _logger.LogProcessingBytes(totalReceived, receiveBuffer.Length, portionSize);
                
                portionSize += increaseStep;
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

        private async Task<Packet> SendPacket(Packet sendPacket, int receivePacketSize)
        {
            var dataSerializedPacket = _byteSerializer.Serialize(sendPacket);

            var dataReceived = await _bytesSender.AcceptedSend(dataSerializedPacket, receivePacketSize);

            return _byteSerializer.Deserialize(dataReceived);
        }
    }
}
