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
    internal class ExecuteBufferCommandHandler : ExecuteCommandHandler
    {
        private readonly IRequestHandlerFactory _requestHandlerFactory;
        private readonly IBytesSerializer _bytesSerializer;

        public ExecuteBufferCommandHandler(
            ServerSessionContext serverSessionContext, 
            IRequestHandlerFactory requestHandlerFactory,
            IPacketFactory packetFactory, 
            IBytesSerializer bytesSerializer,
            ITransportHandler transportHandler) 
            : base(
                serverSessionContext, 
                packetFactory, 
                transportHandler)
        {
            _requestHandlerFactory = requestHandlerFactory;
            _bytesSerializer = bytesSerializer;
        }

        protected override async Task<byte[]> HandleCommand(Packet packet)
        {
            var requestBytes = ServerSessionContext.ReceiveBuffer.GetAll();

            var request = _bytesSerializer.Deserialize<Request>(requestBytes);

            var response = await _requestHandlerFactory
                .CreateRequestHandler()
                .HandleRequest(request);
            
            return _bytesSerializer.Serialize(response);
        }
    }
}
