using System.Threading.Tasks;
using NetworkSocketServer.TransportLayer.Packets;

namespace NetworkSocketServer.TransportLayer.PacketHandler
{
    internal interface IServerPacketHandler
    {
        Task<bool> HandlePacket(Packet packet);
    }
}
