using System.Threading.Tasks;
using NetworkSocketServer.TransportLayer.Packets;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler
{
    internal interface IServerPacketHandler
    {
        Task<bool> HandlePacket(Packet packet);
    }
}
