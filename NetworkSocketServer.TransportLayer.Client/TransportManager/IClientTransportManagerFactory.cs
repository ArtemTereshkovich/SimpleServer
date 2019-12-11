using System;
using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;
using NetworkSimpleServer.NetworkLayer.Core.Logger;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager
{
    public interface IClientTransportManagerFactory
    {
        IClientTransportManager Create(
            IClientTransportHandler clientTransportHandler,
            ILogger logger,
            Guid sessionId);
    }
}
