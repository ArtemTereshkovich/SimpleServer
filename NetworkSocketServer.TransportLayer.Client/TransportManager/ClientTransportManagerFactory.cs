using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSocketServer.TransportLayer.Client.ConnectionManager;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager
{
    public class ClientTransportManagerFactory : IClientTransportManagerFactory
    {
        private readonly RetrySettings _retrySettings;

        public ClientTransportManagerFactory(RetrySettings retrySettings)
        {
            _retrySettings = retrySettings;
        }

        public IClientTransportManager Create(ClientConnectionManager clientConnectionManager)
        {
            var byteSerializer = new BinaryFormatterByteSerializer();
            var packetFactory = new PacketFactory(clientConnectionManager.SessionContext.SessionId);
            var logger = new ConsoleLogger();

            return new ClientTransportManager(logger, packetFactory, );
        }
    }
}
