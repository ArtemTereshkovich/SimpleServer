using System;
using System.Net.Sockets;

namespace NetworkSocketServer.Network.Tcp.KeepAlive
{
    internal class WindowsSocketOptionsAccessor : ISocketOptionsAccessor
    {
        public void SetKeepAliveOptions(Socket tcpSocket, SocketKeepAliveOptions options)
        {
            if (tcpSocket.SocketType != SocketType.Dgram)
            {
                throw new ArgumentException(nameof(tcpSocket));
            }

            tcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
            tcpSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, options.KeepAliveTime);
            tcpSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, options.KeepAliveInterval);
        }
    }
}
