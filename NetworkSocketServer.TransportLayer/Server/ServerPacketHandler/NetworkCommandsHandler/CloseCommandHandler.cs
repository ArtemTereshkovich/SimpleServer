using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.Packets;
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

        public Task<bool> Handle(Packet packet)
        {
            _transportHandler.Close();

            return Task.FromResult(false);
        }
    }
}
