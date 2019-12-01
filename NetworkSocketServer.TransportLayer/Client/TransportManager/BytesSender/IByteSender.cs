using System.Threading.Tasks;

namespace NetworkSocketServer.TransportLayer.Client.ServiceHandlers.RequestExecutor.BytesSender
{
    interface IBytesSender
    {
        Task<byte[]> AcceptedSend(byte[] bytes);
    }
}
