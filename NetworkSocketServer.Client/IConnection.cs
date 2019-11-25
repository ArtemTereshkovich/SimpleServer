using System.Net;
using NetworkSocketServer.DTO.Requests;

namespace NetworkSocketServer.Client
{
    public interface IConnection
    {
        bool Connected { get; }

        void Connect(IPEndPoint endPoint);

        void Send(byte[] bytes);

        void Send(Request command);

        byte[] Receive();

        void Disconect();
    }
}
