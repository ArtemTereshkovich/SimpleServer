using System;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor
{
    public class RetrySettings
    {
        public int CountReconnect { get; set; }

        public TimeSpan ReconnectPeriod { get; set; }

        public TimeSpan TimeOutAnswer { get; set; }
    }
}
