using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Server.AcceptorDispatcher;

namespace NetworkSocketServer.NetworkLayer.Server.AcceptorDispatcher
{
    internal interface IAcceptorDispatcher : INetworkAcceptorSubscriber
    {
        void StartListen();

        void StopListen();
    }
}
