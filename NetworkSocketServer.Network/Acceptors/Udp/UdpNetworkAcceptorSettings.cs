using System.Net;

namespace NetworkSocketServer.NetworkLayer.Acceptors.Udp
{
    public class UdpNetworkAcceptorSettings
    {
        public IPAddress ListenIpAddress { get; set; }

        public int ListenPort { get; set; }
    }
}
