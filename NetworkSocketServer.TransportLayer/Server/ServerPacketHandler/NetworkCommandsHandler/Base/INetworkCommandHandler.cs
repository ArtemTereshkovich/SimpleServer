using System.Threading.Tasks;
using NetworkSocketServer.TransportLayer.Packets;

namespace NetworkSocketServer.TransportLayer.PacketHandler.NetworkCommandsHandler.Base
{
    internal interface INetworkCommandHandler
    {
        Task<bool> Handle(Packet packet);
    }
}
