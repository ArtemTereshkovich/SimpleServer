using NetworkSimpleServer.NetworkLayer.Core;
using NetworkSimpleServer.NetworkLayer.Core.Packets.Formatter;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSimpleServer.NetworkLayer.Server.TransportHandler.Factory;
using NetworkSocketServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.NetworkLayer.Core.TransportHandler.Tcp;

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
