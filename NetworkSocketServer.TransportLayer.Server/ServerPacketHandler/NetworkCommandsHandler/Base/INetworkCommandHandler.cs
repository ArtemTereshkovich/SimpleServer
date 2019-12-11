using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Packets;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base
{
    internal interface INetworkCommandHandler
    {
        Task<bool> Handle(Packet clientPacket);
    }
}
