using System;
using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;
using NetworkSocketServer.TransportLayer.Core.Serializer;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager
{
    public class ClientTransportManagerFactory : IClientTransportManagerFactory
    {
        public IClientTransportManager Create(
            IClientTransportHandler clientTransportHandler,
            ILogger logger,
            Guid sessionId)
        {
            var byteSerializer = new BinaryFormatterBytesSerializer();
            var packetFactory = new PacketFactory(sessionId);

            return new ClientTransportManager(
                logger, 
                packetFactory,
                clientTransportHandler,
                byteSerializer);
        }
    }
}
