using NetworkSocketServer.NetworkLayer.Acceptors;
using NetworkSocketServer.NetworkLayer.Acceptors.Tcp;
using NetworkSocketServer.NetworkLayer.Connectors;

namespace NetworkSocketServer.NetworkLayer.TransportHandler.Factories
{
    internal class BlockingTransportHandlerFactory : ITransportHandlerFactory
    {
        public ITransportHandler CreateTransportHandler(INetworkAcceptor acceptor)
        {
            if (acceptor is TcpKeepAliveNetworkAcceptor)
            {
                return new TcpBlockingTransportHandler();
            }
            else
            {
                return new UDPBlockingReceiveTransportHandler();
            }
        }

        public ITransportHandler CreateTransportHandler(ConnectionType connectionType)
        {
            if (connectionType == ConnectionType.Tcp)
            {
                return  new TcpBlockingTransportHandler();
            }
            else
            {
                return new UDPBlockingReceiveTransportHandler();
            }
        }
    }
}
