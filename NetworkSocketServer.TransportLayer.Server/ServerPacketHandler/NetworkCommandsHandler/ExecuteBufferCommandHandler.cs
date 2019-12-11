using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler
{
    internal class ExecuteBufferCommandHandler : ExecuteCommandHandler
    {
        private readonly IRequestHandlerFactory _requestHandlerFactory;

        public ExecuteBufferCommandHandler(
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
            var requestBytes = ServerSessionContext.ReceiveBuffer.GetAll();

            var request = ByteSerializer.Deserialize<Request>(requestBytes);

            var response = await _requestHandlerFactory
                .CreateRequestHandler()
                .HandleRequest(request);
            
            return ByteSerializer.Serialize(response);
        }
    }
}
