using System;

namespace NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler
{
    public class ClientTransportHandlerRetrySettings
    {
        public int CountReconnect { get; set; }

        public TimeSpan ReconnectPeriod { get; set; }

        public TimeSpan TimeOutAnswer { get; set; }
    }
}
