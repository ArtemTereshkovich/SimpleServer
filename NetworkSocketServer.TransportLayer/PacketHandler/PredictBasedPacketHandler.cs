using System;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.DTO;
using NetworkSocketServer.TransportLayer.ServiceHandlers;

namespace NetworkSocketServer.TransportLayer.PacketHandler
{
    class PredictBasedPacketHandler : IPacketHandler
    {
        private readonly IRequestHandlerFactory _requestHandlerFactory;
        private readonly ITransportHandler _transportHandler;
        private readonly SessionContext _sessionContext;

        public PredictBasedPacketHandler(
            IRequestHandlerFactory requestHandlerFactory,
            ITransportHandler transportHandler,
            SessionContext sessionContext)
        {
            _requestHandlerFactory = requestHandlerFactory;
            _transportHandler = transportHandler;
            _sessionContext = sessionContext;
        }

        public Task HandlePacket(Packet packet)
        {
            throw new NotImplementedException();
        }
    }
}
