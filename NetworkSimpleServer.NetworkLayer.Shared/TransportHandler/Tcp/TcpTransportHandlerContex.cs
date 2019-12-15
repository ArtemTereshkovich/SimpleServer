using System.Net.Sockets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.Core.TransportHandler.Tcp
{
    public class TcpTransportHandlerContext : TransportHandlerContext
    {
        public Socket AcceptedSocket { get; set; }

        public int ServiceId { get; set; }
    }
}
