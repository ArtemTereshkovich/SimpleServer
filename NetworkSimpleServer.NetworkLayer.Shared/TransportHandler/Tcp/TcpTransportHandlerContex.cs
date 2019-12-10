using System.Net.Sockets;

namespace NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Tcp
{
    public class TcpTransportHandlerContext : TransportHandlerContext
    {
        public Socket AcceptedSocket { get; set; }
    }
}
