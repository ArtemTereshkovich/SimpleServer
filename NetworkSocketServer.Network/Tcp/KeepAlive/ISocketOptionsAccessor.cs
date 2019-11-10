using System.Net.Sockets;

namespace NetworkSocketServer.Network.Tcp.KeepAlive
{
    internal interface ISocketOptionsAccessor
    {
        void SetKeepAliveOptions(Socket tcpSocket, SocketKeepAliveOptions options);
    }
}
