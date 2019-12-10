using System;
using System.Linq;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.TransportLayer.Client.ConnectionManager;
using NetworkSocketServer.TransportLayer.Client.Logger;
using NetworkSocketServer.TransportLayer.Client.RequestExecutor;
using NetworkSocketServer.TransportLayer.Client.TransportManager.BytesSender;
using NetworkSocketServer.TransportLayer.Client.TransportManager.Handlers;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager
{
    class ClientTransportManager : IClientTransportManager
    {
        private readonly IClientLogger _clientLogger;
        private readonly ClientConnectionManager _clientConnectionManager;
        private readonly IPacketFactory _packetFactory;
        private readonly IByteSerializer _byteSerializer;
        private readonly IBytesSender _bytesSender;

        public ClientTransportManager(
            IClientLogger clientLogger,
            ClientConnectionManager clientConnectionManager, 
            IByteSerializer byteSerializer,
            IPacketFactory packetFactory,
            RetrySettings retrySettings)
        {
            _bytesSender = new PollyAcceptedBytesSender(clientConnectionManager, retrySettings);
            _clientLogger = clientLogger;
            _clientConnectionManager = clientConnectionManager;
            _byteSerializer = byteSerializer;
            _packetFactory = packetFactory;
        }

        public async Task<Response> SendRequest(Request request)
        {
            var requestBytes = _byteSerializer.Serialize(request);

            var executePacket = await ProcessExecutePacket(requestBytes);

            var answerPacket = await SendPacket(executePacket, 
                _clientConnectionManager.SessionContext.PacketSizeThreshold + 36);

            var responseBytes =  await HandleExecuteAnswer(answerPacket);

            return _byteSerializer.Deserialize<Response>(responseBytes);
        }

        private async Task<Packet> ProcessExecutePacket(byte[] requestBytes)
        {
            if (requestBytes.Length <= _clientConnectionManager.SessionContext.PacketSizeThreshold)
            {
                var requestLength = requestBytes.Length;

                Array.Resize(ref requestBytes, _clientConnectionManager.SessionContext.PacketSizeThreshold);

                return _packetFactory.CreateExecutedInPayload(requestBytes, requestLength);
            }
            else
            {
                var handler = new BufferedRequestHandler(
                    _clientConnectionManager.SessionContext.PacketSizeThreshold,
                    _packetFactory,
                    _byteSerializer,
                    _bytesSender,
                    _clientLogger);

                return await handler.ProvideRequestToServerBuffer(requestBytes);
            }
        }

        private async Task<Packet> SendPacket(Packet packet, int receivedPacketSize)
        {
            var executeBytes = _byteSerializer.Serialize(packet);

            var answerPacket = await _bytesSender.AcceptedSend(executeBytes, receivedPacketSize);

            return _byteSerializer.Deserialize(answerPacket);
        }

        private async Task<byte[]> HandleExecuteAnswer(Packet answerPacket)
        {
            if (answerPacket.PacketServerResponse == PacketServerResponse.ResultInPayload)
            {
                var payload = answerPacket.Payload.Take(answerPacket.PayloadSize).ToArray();

                return payload;
            }
            else
            {
                var handler = new BufferedResponseHandler(
                    _clientConnectionManager.SessionContext.PacketSizeThreshold,
                    _packetFactory,
                    _byteSerializer,
                    _bytesSender,
                    _clientLogger);

                return await handler.GetResponseFromServerBuffer(answerPacket.BufferOffset, 5);
            }
        }
    }
}
