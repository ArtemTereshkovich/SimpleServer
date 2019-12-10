using System.Net.Sockets;

namespace NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Udp
{
    public class UdpTransportHandlerContext : TransportHandlerContext
    {
        public Socket AcceptedSocket { get; set; }
    }
}
