using System;
using System.Net;

namespace NetworkSimpleServer.NetworkLayer.Core.Logger
{
    public interface ILogger
    {
        void LogDisconnectEvent();

        void LogReconnectEvent(IPEndPoint address);

        void LogConnectEvent(IPEndPoint address);

        void LogProcessingBytes(int byteProcessed, int totalBytes, int packetPayloadSize);

        void LogErrorException(Exception exception);
    }
}
