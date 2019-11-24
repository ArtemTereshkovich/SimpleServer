using System.Net;

namespace NetworkSocketServer.Client
{
    public interface IConnection
    {
        bool Connected { get; }

        void Connect(IPEndPoint endPoint);

        void Send(byte[] bytes);

        void Send(Commands.Command command);

        byte[] Receive();

        void Disconect();
    }
}
