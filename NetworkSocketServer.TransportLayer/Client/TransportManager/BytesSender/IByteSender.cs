using System.Threading.Tasks;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager.BytesSender
{
    interface IBytesSender
    {
        Task<byte[]> AcceptedSend(byte[] bytes, int reveiveSize);
    }
}
