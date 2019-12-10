using System;
using System.Net;

namespace NetworkSocketServer.TransportLayer.Client.Logger
{
    public interface IClientLogger
    {
        void LogDisconnectEvent();

        void LogReconnectEvent(IPEndPoint address);

        void LogConnectEvent(IPEndPoint address);

        void LogProcessingBytes(int byteProcessed, int totalBytes, int packetPayloadSize);

        void LogErrorException(Exception exception);
    }
}
