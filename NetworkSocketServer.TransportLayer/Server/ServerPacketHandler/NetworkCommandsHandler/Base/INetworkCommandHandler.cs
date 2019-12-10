using System.Threading.Tasks;
using NetworkSocketServer.TransportLayer.Packets;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base
{
    internal interface INetworkCommandHandler
    {
        Task<bool> Handle(Packet clientPacket);
    }
}
