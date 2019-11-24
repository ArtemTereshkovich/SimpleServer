using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.Acceptors
{
    internal interface INetworkAcceptor
    {
        void Open();

        bool IsHaveNewConnection();

        Task AcceptConnection(ITransportHandler transportHandler);

        void Close();
    }
}