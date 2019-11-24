using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkSocketServer.NetworkLayer.TransportHandler
{
    public interface ITransportHandler : IDisposable
    {
        int ReceiveFragmentSize { get; set; }

        void Activate(Socket socket);

        Task Send(byte[] array);

        Task<byte[]> Receive();

        Task<byte[]> ReceiveAllAvailable();

        void Close();
    }
}
