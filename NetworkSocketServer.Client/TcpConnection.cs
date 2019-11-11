using System.Net;
using System.Net.Sockets;
using NetworkSocketServer.Messages;

namespace NetworkSocketServer.Client
{
    public class TcpConnection : IConnection
    {
        protected Socket socket;

        public bool Connected => socket.Connected;

        public void Connect(IPEndPoint endPoint)
        {
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            //socket.SetKeepAlive(30, 1);

            socket.Connect(endPoint);
        }

        public void Send(byte[] bytes)
        {
            socket.Send(bytes);
        }

        public void Send(Message message)
        {
            socket.Send(message.Serialize());
        }

        public byte[] Receive()
        {
            var zeroBuffer = new byte[0];
            socket.Receive(zeroBuffer);

            var buffer = new byte[socket.Available];
            socket.Receive(buffer);

            return buffer;
        }

        public void Disconect()
        {
            socket.Disconnect(true);
        }
    }
}
