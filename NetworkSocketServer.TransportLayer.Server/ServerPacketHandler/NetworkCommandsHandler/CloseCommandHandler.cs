using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler
{
    class CloseCommandHandler : INetworkCommandHandler
    {
        private readonly ITransportHandler _transportHandler;
        private readonly ILogger _logger;

        public CloseCommandHandler(ITransportHandler transportHandler, ILogger logger)
        {
            _transportHandler = transportHandler;
            _logger = logger;
        }

        public Task<bool> Handle(Packet clientPacket)
        {
            _transportHandler.Close();

            _logger.LogDisconnectEvent();

            return Task.FromResult(false);
        }
    }
}
