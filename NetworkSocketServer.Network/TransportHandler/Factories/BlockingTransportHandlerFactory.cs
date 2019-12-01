using NetworkSocketServer.NetworkLayer.Acceptors;
using NetworkSocketServer.NetworkLayer.Acceptors.Tcp;
using NetworkSocketServer.NetworkLayer.TransportHandler.NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.TransportHandler.Factories
{
    internal class BlockingTransportHandlerFactory : ITransportHandlerFactory
    {
        public ITransportHandler CreateTransportHandler(INetworkAcceptor acceptor)
        {
            if (acceptor is TcpKeepAliveNetworkAcceptor)
            {
                return new TcpBlockingReceiveTransportHandler();
            }
            else
            {
                return new UDPBlockingReceiveTransportHandler();
            }
        }
    }
}
