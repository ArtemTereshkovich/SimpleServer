using System.Net;
using NetworkSocketServer.Messages;

namespace NetworkSocketServer.Client
{
    public interface IConnection
    {
        bool Connected { get; }

        void Connect(IPEndPoint endPoint);

        void Send(byte[] bytes);

        void Send(Message message);

        byte[] Receive();

        void Disconect();
    }
}
