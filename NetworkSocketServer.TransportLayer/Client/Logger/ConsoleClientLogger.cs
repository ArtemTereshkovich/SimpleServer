using System;
using System.Net;

namespace NetworkSocketServer.TransportLayer.Client.Logger
{
    class ConsoleClientLogger : IClientLogger
    {
        public void LogDisconnectEvent()
        {
            Console.WriteLine("Succesfully Disconnected");
        }

        public void LogReconnectEvent(IPEndPoint address)
        {
            Console.WriteLine($"Succesfully reconnected at:{address}");
        }

        public void LogConnectEvent(IPEndPoint address)
        {
            Console.WriteLine($"Succesfully connected at:{address}");
        }
        
        public void LogProcessingBytes(int byteProcessed, int totalBytes)
        {
            throw new System.NotImplementedException();
        }

        public void LogErrorException(Exception exception)
        {
            Console.WriteLine($"Error happend: {exception.Message}. Exception type: {exception.GetType().Name}. StackTrace: {exception.StackTrace}");
        }
    }
}
