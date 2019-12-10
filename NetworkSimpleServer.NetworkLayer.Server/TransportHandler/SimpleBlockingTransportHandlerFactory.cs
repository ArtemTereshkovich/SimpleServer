using NetworkSimpleServer.NetworkLayer.Core;
using NetworkSimpleServer.NetworkLayer.Core.Packets.Serializer;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Tcp;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Udp;
using NetworkSimpleServer.NetworkLayer.Server.Acceptors;
using NetworkSimpleServer.NetworkLayer.Server.Acceptors.Tcp;
using NetworkSimpleServer.NetworkLayer.Server.TransportHandler.Factory;

namespace NetworkSimpleServer.NetworkLayer.Server.TransportHandler
{
    class SimpleBlockingTransportHandlerFactory : IServerTransportHandlerFactory
    {
        public ITransportHandler CreateTransportHandler(INetworkAcceptor acceptor)
        {
            if (acceptor is TcpKeepAliveNetworkAcceptor)
            {
                return new TcpBlockingReceiveTransportHandler(
                    new ManualPacketSerializer(), 
                    PacketConstants.PacketThresholdSize);
            }
            else
            {
                return new UdpCycledCheckTransportHandler(
                    new ManualPacketSerializer(), 
                    PacketConstants.PacketThresholdSize);
            }
        }
    }
}
