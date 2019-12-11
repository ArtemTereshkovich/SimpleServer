using System.Linq;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base;

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
