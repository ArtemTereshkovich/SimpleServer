using System;
using System.Net.Sockets;

namespace NetworkSocketServer.NetworkLayer.TransportHandler
{
    public interface ITransportHandler : IDisposable
    {
        void Activate(Socket socket);

        void Send(byte[] array);

        void ClearReceiveBuffer();

        byte[] Receive();

        byte[] Receive(int length, bool eraseException);

        void Close();
    }
}
