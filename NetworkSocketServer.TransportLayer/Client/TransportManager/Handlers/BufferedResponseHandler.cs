using System.Linq;
using System.Threading.Tasks;
using NetworkSocketServer.TransportLayer.Client.Logger;
using NetworkSocketServer.TransportLayer.Client.ServiceHandlers.RequestExecutor.BytesSender;
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
            byte[] file = new byte[0];

            int fileSize = bufferSize;

            int receivedBytes = 0;

            int receivedBytesPortition = _packetSizeThreshold;
            int receivedBytePortitionStep = increaseStep;

            while (receivedBytes < fileSize)
            {
                int offset = receivedBytes + receivedBytesPortition > fileSize
                    ? fileSize - receivedBytes
                    : receivedBytesPortition;

                var requestPacket = _packetFactory.CreateRead(receivedBytes, offset);

                var requestSer = _byteSerializer.Serialize(requestPacket);

                var dataBytes = await _bytesSender.AcceptedSend(requestSer);

                var dataPacket = _byteSerializer.Deserialize(dataBytes);

                file = file.Concat(dataPacket.Payload).ToArray();

                receivedBytes += dataPacket.Payload.Length;

                _clientLogger.LogProcessingBytes(receivedBytes, fileSize);

                receivedBytesPortition += receivedBytePortitionStep;
            }

            return file;
        }
    }
}
