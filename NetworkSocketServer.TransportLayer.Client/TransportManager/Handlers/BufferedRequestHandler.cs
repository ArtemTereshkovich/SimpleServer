using System;
using System.Threading.Tasks;

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

        public async Task<Packet> ProvideRequestToServerBuffer(byte[] request)
        {
            using var sendBuffer = InitializeBuffer(request);
            
            int totalSend = 0;

            int portionSize = _packetSizeThreshold;

            while (totalSend < sendBuffer.Length)
            {
                portionSize = GetPortionSize(sendBuffer, totalSend, portionSize);
                
                var bytesToSend = sendBuffer.Get(totalSend, portionSize);

                var dataPacket = CreateSendPacket(bytesToSend, portionSize, sendBuffer, totalSend);

                await SendPacket(dataPacket, portionSize + 36);

                totalSend += portionSize;

                _clientLogger.LogProcessingBytes(totalSend, sendBuffer.Length, portionSize);
            }

            return _packetFactory.CreateExecutedInBuffer(sendBuffer.Length);
        }

        private Packet CreateSendPacket(byte[] bytesToSend, int portionSize, IBuffer sendBuffer, int totalSend)
        {
            var byteToSendLength = bytesToSend.Length;

            Array.Resize(ref bytesToSend, portionSize);

            var dataPacket = _packetFactory.CreateWrite(
                bytesToSend,
                sendBuffer.Length,
                totalSend,
                byteToSendLength);

            return dataPacket;
        }

        private async Task SendPacket(Packet dataPacket, int receivedPacketSize)
        {
            var dataSerializedPacket = _byteSerializer.Serialize(dataPacket);

            await _bytesSender.AcceptedSend(dataSerializedPacket, receivedPacketSize);
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
