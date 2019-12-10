using System.Net.Sockets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Context;

namespace NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Tcp
{
    public class TcpTransportHandlerContext : TransportHandlerContext
    {
        public Socket AcceptedSocket { get; set; }
    }
}
