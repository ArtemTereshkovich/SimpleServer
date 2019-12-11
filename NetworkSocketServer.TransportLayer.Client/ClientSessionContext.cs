using System;
using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;

namespace NetworkSocketServer.TransportLayer.Client
{
    public class ClientSessionContext
    {
        public IClientTransportHandler ClientTransportHandler { get; private set; }

        public Guid SessionId { get; }

        public ClientSessionContext(
            IClientTransportHandler clientTransportHandler, 
            Guid sessionId)
        {
            ClientTransportHandler = clientTransportHandler;
            SessionId = sessionId;
        }
    }
}
