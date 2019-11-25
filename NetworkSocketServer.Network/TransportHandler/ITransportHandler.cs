using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkSocketServer.NetworkLayer.TransportHandler
{
    public interface ITransportHandler : IDisposable
    {
        void Activate(Socket socket);

        void Send(byte[] array);

        byte[] Receive();

        void Close();
    }
}
