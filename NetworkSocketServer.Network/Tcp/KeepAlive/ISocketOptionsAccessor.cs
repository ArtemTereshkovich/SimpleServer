using System.Net.Sockets;

namespace NetworkSocketServer.Network.Tcp.KeepAlive
{
    internal interface ISocketOptionsAccessor
    {
        void SetOptions(Socket tcpSocket);
    }
}
