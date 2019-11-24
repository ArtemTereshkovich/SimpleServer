using System.Net;

namespace NetworkSocketServer.NetworkLayer.Acceptors.Tcp
{
    public class TcpNetworkAcceptorSettings
    {
        public IPAddress ListenIpAddress { get; set; }

        public int ListenPort { get; set; }

        public int ListenMaxBacklogConnection { get; set; }
    }
}
