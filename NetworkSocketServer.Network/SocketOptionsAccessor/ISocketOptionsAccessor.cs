using System.Net.Sockets;

namespace NetworkSocketServer.Network.SocketOptionsAccessor
{
    internal interface ISocketOptionsAccessor
    {
        void SetOptions(Socket tcpSocket);
    }
}
