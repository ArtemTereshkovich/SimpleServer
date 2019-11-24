using System.Net.Sockets;

namespace NetworkSocketServer.NetworkLayer.SocketOptionsAccessor
{
    internal interface ISocketOptionsAccessor
    {
        void SetOptions(Socket tcpSocket);
    }
}
