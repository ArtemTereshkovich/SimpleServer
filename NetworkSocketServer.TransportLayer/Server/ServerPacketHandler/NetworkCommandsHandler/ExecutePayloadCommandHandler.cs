using System.Linq;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base;
using NetworkSocketServer.TransportLayer.ServiceHandlers;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler
{
    internal class ExecutePayloadCommandHandler : ExecuteCommandHandler
    {
        private readonly IRequestHandlerFactory _requestHandlerFactory;

        public ExecutePayloadCommandHandler(
            ServerSessionContext serverSessionContext, 
            IRequestHandlerFactory requestHandlerFactory,
            IPacketFactory packetFactory, 
            IByteSerializer byteSerializer, 
            ITransportHandler transportHandler) 
            : base(
                serverSessionContext, 
                packetFactory,
                byteSerializer,
                transportHandler)
        {
            _requestHandlerFactory = requestHandlerFactory;
        }

        protected override async Task<byte[]> HandleCommand(Packet packet)
        {
            var payload = packet.Payload.Take(packet.PayloadSize).ToArray();

            var request = ByteSerializer.Deserialize<Request>(payload);

            var response = await _requestHandlerFactory.CreateRequestHandler().HandleRequest(request);

            return ByteSerializer.Serialize(response);
        }
    }
}
