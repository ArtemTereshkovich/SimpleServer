using System.Net.Sockets;

namespace NetworkSimpleServer.NetworkLayer.Core.SocketOptionsAccessor
{
    public interface ISocketOptionsAccessor
    {
        void SetOptions(Socket tcpSocket);
    }
}
