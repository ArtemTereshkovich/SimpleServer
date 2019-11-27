using System.Threading.Tasks;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor.BytesSender
{
    interface IBytesSender
    {
        Task<byte[]> AcceptedSend(byte[] bytes);
    }
}
