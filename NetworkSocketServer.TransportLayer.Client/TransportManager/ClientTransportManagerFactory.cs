using NetworkSocketServer.TransportLayer.Client.ConnectionManager;
using NetworkSocketServer.TransportLayer.Client.Logger;
using NetworkSocketServer.TransportLayer.Client.RequestExecutor;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;

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
            var logger = new ConsoleClientLogger();

            return new ClientTransportManager(
                    logger,
                    clientConnectionManager, 
                    byteSerializer,
                    packetFactory,
                    _retrySettings);
        }
    }
}
