using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Packets;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler
{
    internal interface IServerPacketHandler
    {
        Task<bool> HandlePacket(Packet packet);
    }
}
