using System.Threading.Tasks;
using NetworkSocketServer.Network.TransportHandler;

namespace NetworkSocketServer.Network.ThreadSet
{
    internal interface IThreadSet
    {
        void Execute(INetworkServiceHandler serviceHandler, Task<ITransportHandler> transportHandler);
    }
}
