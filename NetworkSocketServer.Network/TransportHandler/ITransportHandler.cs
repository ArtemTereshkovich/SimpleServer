using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkSocketServer.Network.TransportHandler
{
    public interface ITransportHandler
    {
        void Activate(Socket socket);

        Task Send(byte[] array);

        Task<byte[]> Receive();

        void Close();
    }
}
