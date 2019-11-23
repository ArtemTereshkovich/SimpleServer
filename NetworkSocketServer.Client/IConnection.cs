using System.Net;

namespace NetworkSocketServer.Client
{
    public interface IConnection
    {
        bool Connected { get; }

        void Connect(IPEndPoint endPoint);

        void Send(byte[] bytes);

        void Send(Messages.Command command);

        byte[] Receive();

        void Disconect();
    }
}
