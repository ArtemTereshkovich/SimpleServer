using System.Net.Sockets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Context;

namespace NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Udp
{
    public class UdpTransportHandlerContext : TransportHandlerContext
    {
        public Socket AcceptedSocket { get; set; }
    }
}
