using System;
using NetworkSocketServer.NetworkLayer.Connectors;
using NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.TransportLayer
{
    public class ClientSessionContext
    {
        public int PacketSizeThreshold { get; } = 1024;
        public NetworkConnectorSettings CurrentConnectorSettings { get; }

        public ITransportHandler TransportHandler { get; private set; }

        public Guid SessionId { get; }

        public ClientSessionContext(
            ITransportHandler transportHandler, 
            Guid sessionId, 
            NetworkConnectorSettings currentConnectorSettings)
        {
            TransportHandler = transportHandler;
            SessionId = sessionId;
            CurrentConnectorSettings = currentConnectorSettings;
        }

        public void ChangeTransportHandler(ITransportHandler transportHandler)
        {
            TransportHandler = transportHandler;
        }
    }
}
