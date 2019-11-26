using System;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.Acceptors.Udp
{
    internal class UdpThreadNetworkAcceptor : INetworkAcceptor
    {
        private readonly UdpNetworkAcceptorSettings _acceptorSettings;

        public UdpThreadNetworkAcceptor(UdpNetworkAcceptorSettings acceptorSettings)
        {
            _acceptorSettings = acceptorSettings;
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public bool IsHaveNewConnection()
        {
            throw new NotImplementedException();
        }

        public Task AcceptConnection(ITransportHandler transportHandler)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
