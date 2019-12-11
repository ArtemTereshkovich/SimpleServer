using System.Linq;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;
using NetworkSocketServer.TransportLayer.Core.Serializer;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler
{
    internal class ExecutePayloadCommandHandler : ExecuteCommandHandler
    {
        private readonly IRequestHandlerFactory _requestHandlerFactory;
        private readonly IBytesSerializer _byteSerializer;

        public ExecutePayloadCommandHandler(
            ServerSessionContext serverSessionContext, 
            IRequestHandlerFactory requestHandlerFactory,
            IPacketFactory packetFactory, 
            IBytesSerializer byteSerializer, 
            ITransportHandler transportHandler) 
            : base(
                serverSessionContext, 
                packetFactory,
                transportHandler)
        {
            _requestHandlerFactory = requestHandlerFactory;
            _byteSerializer = byteSerializer;
        }

        protected override async Task<byte[]> HandleCommand(Packet packet)
        {
            var payload = packet.Payload.Take(packet.PayloadSize).ToArray();

            var request = _byteSerializer.Deserialize<Request>(payload);

            var response = await _requestHandlerFactory.CreateRequestHandler().HandleRequest(request);

            return _byteSerializer.Serialize(response);
        }
    }
}
