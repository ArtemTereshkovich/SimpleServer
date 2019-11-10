using System.Threading.Tasks;

namespace NetworkSocketsServer.Shared
{
    public interface INetworkAcceptor
    {
        void StartListen();

        bool IsHaveNewConnection();

        Task<INetworkConnection> AcceptConnection();

        void StopListen();
    }
}
