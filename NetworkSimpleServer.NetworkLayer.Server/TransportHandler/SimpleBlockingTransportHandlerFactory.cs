using NetworkSimpleServer.NetworkLayer.Core;
using NetworkSimpleServer.NetworkLayer.Core.Packets.Formatter;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Tcp;
using NetworkSimpleServer.NetworkLayer.Server.TransportHandler.Factory;

namespace NetworkSocketServer.NetworkLayer.Server.TransportHandler
{
    class SimpleBlockingTransportHandlerFactory : IServerTransportHandlerFactory
    {
        public ITransportHandler CreateTransportHandler()
        {
            return new TcpBlockingReceiveTransportHandler(
                new ManualPacketByteFormatter(),
                PacketConstants.PacketThresholdSize);
        }
    }
}
