using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler
{
    class CloseCommandHandler : INetworkCommandHandler
    {
        private readonly ITransportHandler _transportHandler;

        public CloseCommandHandler(ITransportHandler transportHandler)
        {
            _transportHandler = transportHandler;
        }

        public Task<bool> Handle(Packet clientPacket)
        {
            _transportHandler.Close();

            return Task.FromResult(false);
        }
    }
}
