using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.TransportLayer.Client.ConnectionManager;
using NetworkSocketServer.TransportLayer.Client.Logger;
using NetworkSocketServer.TransportLayer.Client.RequestExecutor;
using NetworkSocketServer.TransportLayer.Client.ServiceHandlers.RequestExecutor.BytesSender;
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

            var answerPacket = await SendPacket(executePacket);

            var responseBytes =  await HandleExecuteAnswer(answerPacket);

            return _byteSerializer.Deserialize<Response>(responseBytes);
        }

        private async Task<Packet> ProcessExecutePacket(byte[] requestBytes)
        {
            if (requestBytes.Length <= _clientConnectionManager.SessionContext.PacketSizeThreshold * 2)
            {
                return _packetFactory.CreateExecutePayload(requestBytes);
            }
            else
            {
                var handler = new BufferedRequestHandler(
                    _clientConnectionManager.SessionContext.PacketSizeThreshold,
                    _packetFactory,
                    _byteSerializer,
                    _bytesSender,
                    _clientLogger);

                return await handler.ProvideRequestToServerBuffer(requestBytes, 100);
            }
        }

        private async Task<Packet> SendPacket(Packet packet)
        {
            var executeBytes = _byteSerializer.Serialize(packet);

            var answerPacket = await _bytesSender.AcceptedSend(executeBytes);

            return _byteSerializer.Deserialize(answerPacket);
        }

        private async Task<byte[]> HandleExecuteAnswer(Packet answerPacket)
        {
            if (answerPacket.PacketServerResponse == PacketServerResponse.ResultInPayload)
            {
                return answerPacket.Payload;
            }
            else
            {
                var handler = new BufferedResponseHandler(
                    _clientConnectionManager.SessionContext.PacketSizeThreshold,
                    _packetFactory,
                    _byteSerializer,
                    _bytesSender,
                    _clientLogger);

                return await handler.GetResponseFromServerBuffer(answerPacket.Offset, 100);
            }
        }
    }
}
