using System.Net;

namespace NetworkSocketServer.Network.Tcp
{
    internal class TcpNetworkAcceptorSettings
    {
        public IPAddress ListenIpAddress { get; set; }

        public int ListenPort { get; set; }

        public int ListenMaxBacklogConnection { get; set; }
    }
}
