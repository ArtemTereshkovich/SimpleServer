using System.Threading.Tasks;
using NetworkSocketServer.TransportLayer.DTO;

namespace NetworkSocketServer.TransportLayer.PacketHandler
{
    interface IPacketHandler
    {
        Task HandlePacket(Packet packet);
    }
}
