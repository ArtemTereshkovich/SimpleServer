using System;
using System.Text;
using System.Threading.Tasks;
using NetworkSocketServer.TransportLayer.DTO;
using NetworkSocketServer.TransportLayer.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor.BytesSender;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor
{
    class RequestExecutor : IRequestExecutor
    {
        private readonly NetworkClientManager.NetworkClientManager _networkClientManager;
        private readonly RetrySettings _retrySettings;
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
            _retrySettings = retrySettings;
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
            var answerPacket = _byteSerializer.Deserialize<Packet>(answerBytes);

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

        private async Task<byte[]> HandleResponseResultInBuffer(Packet answerPacket)
        {
            throw new NotImplementedException();
        }

        private void CheckPacket(Packet packet)
        {
            if (packet.PacketServerResponse == PacketServerResponse.Error)
            {
                string errorMessage = Encoding.ASCII.GetString(packet.Payload);

                throw new InvalidOperationException(errorMessage);
            }
        }

        private Task<Packet> CreateBufferedExecutPacket(byte[] request)
        {
            throw new NotImplementedException();
        }
    }
}
