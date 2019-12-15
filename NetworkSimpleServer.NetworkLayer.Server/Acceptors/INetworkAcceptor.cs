using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.NetworkLayer.Core.TransportHandler;

namespace NetworkSimpleServer.NetworkLayer.Server.Acceptors
{
    internal interface INetworkAcceptor
    {
        void Open();

        bool IsHaveNewConnection();

        Task AcceptConnection(ITransportHandler transportHandler);

        void Close();
    }
}