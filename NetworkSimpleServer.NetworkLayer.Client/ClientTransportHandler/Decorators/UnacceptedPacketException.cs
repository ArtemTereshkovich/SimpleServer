using System;

namespace NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler
{
    public class UnacceptedPacketException : Exception
    {
        public UnacceptedPacketException() : base()
        {

        }

        public UnacceptedPacketException(string message) : base(message)
        {

        }
    }
}
