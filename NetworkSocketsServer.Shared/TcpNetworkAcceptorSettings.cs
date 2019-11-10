using System.Net;

namespace NetworkSocketsServer.Shared
{
    public class TcpNetworkAcceptorSettings
    {
        public IPAddress ListenIpAddress { get; set; }

        public int ListenPort { get; set; }

        public int ListenMaxBacklogConnection { get; set; }
    }
}