using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.PacketHandler.NetworkCommandsHandler.Base;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.Server;
using NetworkSocketServer.TransportLayer.ServiceHandlers;

namespace NetworkSocketServer.TransportLayer.PacketHandler.NetworkCommandsHandler
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
            var request = ByteSerializer.Deserialize<Request>(packet.Payload);

            var response = await _requestHandlerFactory.CreateRequestHandler().HandleRequest(request);

            return ByteSerializer.Serialize(response);
        }
    }
}
