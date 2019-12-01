using System;
using System.Threading.Tasks;
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
            int requestLength = request.Length;

            int sendedBytes = 0;

            int sendedPacketPortition = _packetSizeThreshold;
            int sendedPacketPortitionStep = increaseStep;

            while (sendedBytes < requestLength)
            {
                int offset = sendedBytes + sendedPacketPortition > requestLength
                    ? requestLength - sendedBytes
                    : sendedPacketPortition;

                var arraySegment = new ArraySegment<byte>(request, sendedBytes, offset);

                var dataPacket = _packetFactory.CreateWrite(arraySegment.ToArray(), requestLength, sendedBytes);

                var dataSerializedPacket = _byteSerializer.Serialize(dataPacket);

                await _bytesSender.AcceptedSend(dataSerializedPacket);

                sendedBytes += offset;

                _clientLogger.LogProcessingBytes(sendedBytes, requestLength);

                sendedPacketPortition += sendedPacketPortitionStep;
            }

            return _packetFactory.CreateExecuteBuffer(requestLength);
        }
    }
}
