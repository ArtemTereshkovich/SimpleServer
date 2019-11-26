using System;
using System.Net;

namespace NetworkSocketServer.NetworkLayer.Connectors
{
    public class NetworkConnectorSettings
    {
        public IPEndPoint IpEndPointServer { get; set; }

        public ConnectionType ConnectionType { get; set; }
    }

    public enum ConnectionType
    {
        Tcp,
        Udp
    }
}
