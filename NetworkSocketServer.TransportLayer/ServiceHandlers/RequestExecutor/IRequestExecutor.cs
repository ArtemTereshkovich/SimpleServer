using System.Threading.Tasks;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor
{
    public interface IRequestExecutor
    {
        Task<byte[]> HandleRequest(byte[] request);
    }
}
