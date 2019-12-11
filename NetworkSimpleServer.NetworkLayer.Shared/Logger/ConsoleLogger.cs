using System;
using System.Net;

namespace NetworkSimpleServer.NetworkLayer.Core.Logger
{
    public class ConsoleLogger : ILogger
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
        
        public void LogProcessingBytes(int byteProcessed, int totalBytes, int packetPayloadSize)
        {
            Console.SetCursorPosition(30, Console.CursorTop);

            Console.Write($"{byteProcessed} / {totalBytes} Bytes. Packet payload: {packetPayloadSize}");
        }

        public void LogErrorException(Exception exception)
        {
            Console.WriteLine($"Error happend. Exception type: {exception.GetType().Name}. StackTrace: {exception.StackTrace}");
        }

        public void LogError(string text)
        {
            Console.WriteLine(text);
        }

        public void LogConnectEvent()
        {
            Console.WriteLine("Received new connection");
        }
    }
}
