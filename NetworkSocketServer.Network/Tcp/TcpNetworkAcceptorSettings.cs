using System.Net;

namespace NetworkSocketServer.Network.Tcp
{
    public class TcpNetworkAcceptorSettings
    {
        public IPAddress ListenIpAddress { get; set; }

        public int ListenPort { get; set; }

        public int ListenMaxBacklogConnection { get; set; }
    }
}
