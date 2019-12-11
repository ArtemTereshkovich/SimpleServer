using System;
using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSocketServer.TransportLayer.Core.Buffer;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager.Handlers
{
    class BufferedRequestHandler
    {
        private readonly int _packetPayloadSize;
        private readonly IPacketFactory _packetFactory;
        private readonly IClientTransportHandler _clientTransportHandler;
        private readonly ILogger _logger;

        public BufferedRequestHandler(
            int packetPayloadSize, 
            IPacketFactory packetFactory, 
            IClientTransportHandler clientTransportHandler,
            ILogger logger)
        {
            _packetPayloadSize = packetPayloadSize;
            _packetFactory = packetFactory;
            _clientTransportHandler = clientTransportHandler;
            _logger = logger;
        }

        public Packet ProvideRequestToServerBuffer(byte[] request)
        {
            using var sendBuffer = InitializeBuffer(request);
            
            int totalSend = 0;

            int portionSize = _packetPayloadSize;

            while (totalSend < sendBuffer.Length)
            {
                portionSize = GetPortionSize(sendBuffer, totalSend, portionSize);
                
                var bytesToSend = sendBuffer.Get(totalSend, portionSize);

                var writePacket = CreateSendPacket(bytesToSend, portionSize, sendBuffer, totalSend);

                _clientTransportHandler.AcceptedSend(writePacket);

                totalSend += portionSize;

                _logger.LogProcessingBytes(totalSend, sendBuffer.Length, portionSize);
            }

            return _packetFactory.CreateExecutedInBuffer(sendBuffer.Length);
        }

        private Packet CreateSendPacket(byte[] bytesToSend, int portionSize, IBuffer sendBuffer, int totalSend)
        {
            var byteToSendLength = bytesToSend.Length;

            Array.Resize(ref bytesToSend, _packetPayloadSize);

            var dataPacket = _packetFactory.CreateWrite(
                bytesToSend,
                sendBuffer.Length,
                totalSend,
                byteToSendLength);

            return dataPacket;
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
