using System.Threading.Tasks;
using NetworkSocketServer.Network.TransportHandler;

namespace NetworkSocketServer.Network
{
    internal interface INetworkAcceptor
    {
        void Open();

        bool IsHaveNewConnection();

        Task AcceptConnection(ITransportHandler transportHandler);

        void Close();
    }
}