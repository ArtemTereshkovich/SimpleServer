using System.Net.Sockets;

namespace NetworkSocketsServer.Shared
{
    public class TcpKeepAliveNetworkConnection : INetworkConnection
    {
        private readonly Socket _tcpSocket;
    
        protected TcpKeepAliveNetworkConnection(Socket tcpSocket)
        {
            _tcpSocket = tcpSocket;
        }

        public static TcpKeepAliveNetworkConnection CreateConnectionFromSocket(Socket tcpSocket)
        {
            SetKeepAliveOption(tcpSocket);
            return new TcpKeepAliveNetworkConnection(tcpSocket);
        }

        private static void SetKeepAliveOption(Socket tcpSocket)
        {

        }

        public void Dispose()
        {
            _tcpSocket.Dispose();
        }
    }
}