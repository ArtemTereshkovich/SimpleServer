using System.Threading.Tasks;

namespace NetworkSocketServer.Network
{
    internal interface INetworkAcceptor
    {
        void Open();

        bool IsHaveNewConnection();

        Task AcceptConnection(INetworkServiceHandle serviceHandle);

        void Close();
    }
}