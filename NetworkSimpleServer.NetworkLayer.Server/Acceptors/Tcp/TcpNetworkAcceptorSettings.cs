using System.Net;

namespace NetworkSimpleServer.NetworkLayer.Server.Acceptors.Tcp
{
    public class TcpNetworkAcceptorSettings
    {
        public IPAddress ListenIpAddress { get; set; }

        public int ListenPort { get; set; }

        public int ListenMaxBacklogConnection { get; set; }
    }
}
