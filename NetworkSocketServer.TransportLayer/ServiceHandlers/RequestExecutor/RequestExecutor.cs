using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using NetworkSocketServer.TransportLayer.DTO;
using NetworkSocketServer.TransportLayer.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor.BytesSender;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor
{
    class RequestExecutor : IRequestExecutor
    {
        private readonly NetworkClientManager.NetworkClientManager _networkClientManager;
        private readonly IPacketFactory _packetFactory;
        private readonly IByteSerializer _byteSerializer;
        private readonly IBytesSender _bytesSender;

        public RequestExecutor(
            NetworkClientManager.NetworkClientManager networkClientManager, 
            RetrySettings retrySettings)
        {
            _bytesSender = new PollyAcceptedBytesSender(networkClientManager, retrySettings);
            _byteSerializer = new BinaryFormatterByteSerializer();
            _networkClientManager = networkClientManager;
            _packetFactory = new PacketFactory.PacketFactory(networkClientManager.SessionContext.SessionId);
        }

        public async Task<byte[]> HandleRequest(byte[] request)
        {
            Packet executPacket = null;

            if (request.Length <= _networkClientManager.SessionContext.PacketSizeThreshold * 2)
            {
                executPacket = _packetFactory.CreateExecutePayload(request);
            }
            else
            {
                executPacket = await CreateBufferedExecutPacket(request);
            }

            var executeBytes = _byteSerializer.Serialize(executPacket);
            var answerBytes = await _bytesSender.AcceptedSend(executeBytes);
            var answerPacket = _byteSerializer.Deserialize(answerBytes);

            CheckPacket(answerPacket);

            if (answerPacket.PacketServerResponse == PacketServerResponse.ResultInPayload)
            {
                return answerPacket.Payload;
            }
            else
            {
                return await HandleResponseResultInBuffer(answerPacket);
            }
        }

        private void CheckPacket(Packet packet)
        {
            if (packet.PacketServerResponse == PacketServerResponse.Error)
            {
                string errorMessage = Encoding.ASCII.GetString(packet.Payload);

                throw new InvalidOperationException(errorMessage);
            }
        }

        private async Task<byte[]> HandleResponseResultInBuffer(Packet answerPacket)
        {
            byte[] file = new byte[0];

            int fileSize = answerPacket.Offset;

            int receivedBytes = 0;

            int receivedBytesPortition = _networkClientManager.SessionContext.PacketSizeThreshold;
            int receivedBytePortitionStep = 100;

            while (receivedBytes < fileSize)
            {
                int offset = receivedBytes + receivedBytesPortition > fileSize
                    ? fileSize - receivedBytes
                    : receivedBytesPortition;

                var requestPacket = _packetFactory.CreateRead(receivedBytes, offset);

                var requestSer = _byteSerializer.Serialize(requestPacket);

                var dataBytes = await _bytesSender.AcceptedSend(requestSer);

                var dataPacket = _byteSerializer.Deserialize(dataBytes);

                CheckPacket(dataPacket);

                file = file.Concat(dataPacket.Payload).ToArray();


                Console.WriteLine($"{receivedBytes} / {fileSize}");

                receivedBytes += dataPacket.Payload.Length;

                receivedBytesPortition += receivedBytePortitionStep;
            }

            return file;
        }

        private async Task<Packet> CreateBufferedExecutPacket(byte[] request)
        {
            int requestLength = request.Length;

            int sendedBytes = 0;

            int sendedPacketPortition = _networkClientManager.SessionContext.PacketSizeThreshold;
            int sendedPacketPortitionStep = 100;

            while (sendedBytes < requestLength)
            {
                int offset = sendedBytes + sendedPacketPortition > requestLength
                    ? requestLength - sendedBytes
                    : sendedPacketPortition;

                var arraySegment = new ArraySegment<byte>(request, sendedBytes, offset);

                var dataPacket = _packetFactory.CreateWrite(arraySegment.ToArray(), requestLength, sendedBytes);

                var dataSerializedPacket = _byteSerializer.Serialize(dataPacket);

                var answer = await _bytesSender.AcceptedSend(dataSerializedPacket);

                var answerPacket = _byteSerializer.Deserialize(answer);

                CheckPacket(answerPacket);

                Console.WriteLine($"{sendedBytes} / {requestLength}");

                sendedBytes += offset;

                sendedPacketPortition += sendedPacketPortitionStep;
            }

            return _packetFactory.CreateExecuteBuffer(requestLength);
        }
    }
}
