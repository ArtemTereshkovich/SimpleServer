using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Server.AcceptorDispatcher;

namespace NetworkSocketServer.NetworkLayer.Server.AcceptorDispatcher
{
    internal interface IAcceptorDispatcher : INetworkAcceptorSubscriber
    {
        Task StartListen();

        void StopListen();
    }
}
