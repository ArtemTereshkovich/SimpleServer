using System;

namespace NetworkSocketServer.Network.TransportHandler
{
    public class TransportHandlerRetryOptions
    {
        public int RetryCount { get; set; }

        public TimeSpan RetryInterval { get; set; }
    }
}