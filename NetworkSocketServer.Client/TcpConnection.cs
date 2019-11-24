using System.Net;
using System.Net.Sockets;
using NetworkSocketServer.Commands;

namespace NetworkSocketServer.Client
{
    public class TcpConnection : IConnection
    {
        protected Socket Socket;

        public bool Connected => Socket.Connected;

        public void Connect(IPEndPoint endPoint)
        {
            Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            //socket.SetKeepAlive(30, 1);

            Socket.Connect(endPoint);
        }

        public void Send(byte[] bytes)
        {
            Socket.Send(bytes);
        }

        public void Send(Commands.Command command)
        {
            Socket.Send(command.Serialize());
        }

        public byte[] Receive()
        {
            var buffer = new byte[Socket.Available];
            Socket.Receive(buffer);

            return buffer;
        }

        public void Disconect()
        {
            Socket.Disconnect(true);
        }
    }
}
