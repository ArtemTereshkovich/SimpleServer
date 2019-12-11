using System;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Client.ConnectorDispatcher;
using NetworkSimpleServer.NetworkLayer.Client.Connectors;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.TransportLayer.Client.TransportManager;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;

namespace NetworkSocketServer.TransportLayer.Client.ConnectionManager
{
    public class ClientConnectionManager : IClientConnectionManager
    {
        private readonly ILogger _logger;
        private readonly IConnectorDispatcher _dispatcher;
        private readonly IClientTransportManagerFactory _clientTransportManagerFactory;
        public ClientSessionContext SessionContext { get; private set; }

        public ClientConnectionManager(
            IConnectorDispatcher dispatcher,
            IClientTransportManagerFactory clientTransportManagerFactory,
            ILogger logger)
        {
            _dispatcher = dispatcher;
            _clientTransportManagerFactory = clientTransportManagerFactory;
            _logger = logger;
        }

        public bool IsConnected => SessionContext != null;

        public void Connect(NetworkConnectorSettings connectSettings)
        {
            if (SessionContext != null)
                throw new InvalidOperationException("Session already exist");

            try
            {
                var clientTransportHandler = _dispatcher.CreateClientTransportHandler(connectSettings);

                SessionContext = new ClientSessionContext(clientTransportHandler, Guid.NewGuid());

                _logger.LogConnectEvent(connectSettings.IpEndPointServer);
            }
            catch (Exception exception)
            {
                _logger.LogErrorException(exception);
                SessionContext = null;
            }
        }
        
        public void Disconnect()
        {
            CheckSessionContext();

            SafeCloseSession();

            _logger.LogDisconnectEvent();
        }

        public async Task<Response> SendRequest(Request request)
        {
            CheckSessionContext();

            try
            {
                var clientTransportManager = _clientTransportManagerFactory.Create(this);

                return await clientTransportManager.SendRequest(request);
            }
            catch 
            {
                SafeCloseSession();

                throw;
            }
        }

        private void SafeCloseSession()
        {
            IPacketFactory packetFactory = new PacketFactory(SessionContext.SessionId);

            var closePacket = packetFactory.CreateClosePacket();

            try
            {
                SessionContext.ClientTransportHandler.UnAcceptedSend(closePacket);
            }
            catch (Exception exception)
            {
                _logger.LogErrorException(exception);
            }

            try
            {
                SessionContext.ClientTransportHandler.Close();
            }
            catch (Exception exception)
            {
                _logger.LogErrorException(exception);
            }

            SessionContext = null;
        }

        private void CheckSessionContext()
        {
            if (SessionContext == null)
                throw new InvalidOperationException("No session context");
        }
    }
}
