using System.Net;

namespace NetworkSimpleServer.NetworkLayer.Server.Acceptors.Udp
{
    public class UdpNetworkAcceptorSettings
    {
        public IPAddress ListenIpAddress { get; set; }

        public int ListenPort { get; set; }
    }
}
