using System;

namespace NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler.Decorators
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
