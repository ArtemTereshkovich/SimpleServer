using System.Linq;
using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSocketServer.TransportLayer.Core.Buffer;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager.Handlers
{
    internal class BufferedResponseHandler
    {
        private readonly int _packetSizeThreshold;
        private readonly IPacketFactory _packetFactory;
        private readonly ILogger _logger;
        private readonly IClientTransportHandler _clientTransportHandler;

        public BufferedResponseHandler(
            int packetSizeThreshold, 
            IPacketFactory packetFactory, 
            ILogger logger, 
            IClientTransportHandler clientTransportHandler)
        {
            _packetSizeThreshold = packetSizeThreshold;
            _packetFactory = packetFactory;
            _logger = logger;
            _clientTransportHandler = clientTransportHandler;
        }
        
        public byte[] GetResponseFromServerBuffer(int bufferSize, int increaseStep)
        {
            using var receiveBuffer = InitializeBuffer(bufferSize);

            int totalReceived = 0;

            int portionSize = _packetSizeThreshold;

            while (totalReceived < receiveBuffer.Length)
            {
                portionSize = GetPortionSize(receiveBuffer, totalReceived, portionSize);

                var readPacket = _packetFactory.CreateRead(totalReceived, portionSize);

                var dataPacket = _clientTransportHandler.AcceptedSend(readPacket);

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
    }
}
