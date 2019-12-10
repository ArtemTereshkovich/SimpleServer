using System;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.NetworkLayer.Connectors;
using NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.Client.Logger;
using NetworkSocketServer.TransportLayer.Client.TransportManager;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;

namespace NetworkSocketServer.TransportLayer.Client.ConnectionManager
{
    public class ClientConnectionManager : IClientConnectionManager
    {
        private readonly IClientLogger _logger;
        private readonly IByteSerializer _byteSerializer;
        private readonly IConnectorDispatcher _dispatcher;
        private readonly IClientTransportManagerFactory _clientTransportManagerFactory;
        public ClientSessionContext SessionContext { get; private set; }

        public ClientConnectionManager(
            IConnectorDispatcher dispatcher,
            IClientTransportManagerFactory clientTransportManagerFactory, 
            IClientLogger logger)
        {
            _byteSerializer = new BinaryFormatterByteSerializer();;
            _dispatcher = dispatcher;
            _clientTransportManagerFactory = clientTransportManagerFactory;
            _logger = logger;
        }

        public bool IsConnected => SessionContext != null;

        public async Task Connect(NetworkConnectorSettings connectSettings)
        {
            if (SessionContext != null)
                throw new InvalidOperationException("Session already exist");

            try
            {
                var transportHandler = await _dispatcher.CreateTransportHandler(connectSettings);

                SessionContext = new ClientSessionContext(
                    transportHandler,
                    Guid.NewGuid(),
                    connectSettings);

                _logger.LogConnectEvent(connectSettings.IpEndPointServer);
            }
            catch (Exception exception)
            {
                _logger.LogErrorException(exception);
                SessionContext = null;
            }
        }

        public async Task Reconnect()
        {
            if (SessionContext == null)
                throw new InvalidOperationException("No session context");

            if (SessionContext.TransportHandler is UDPBlockingReceiveTransportHandler udpBlocking)
            {
                udpBlocking.Reconnect(SessionContext.CurrentConnectorSettings);

                _logger.LogReconnectEvent(SessionContext.CurrentConnectorSettings.IpEndPointServer);

                return;
            }

            try
            {
                SessionContext.TransportHandler.Close();
            }
            catch (Exception exception)
            {
                _logger.LogErrorException(exception);
            }

            var transportHandler = await _dispatcher.CreateTransportHandler(
                SessionContext.CurrentConnectorSettings);

            SessionContext.ChangeTransportHandler(transportHandler);

            _logger.LogReconnectEvent(SessionContext.CurrentConnectorSettings.IpEndPointServer);
        }

        public Task Disconnect()
        {
            CheckSessionContext();

            SafeCloseSession();

            _logger.LogDisconnectEvent();

            return Task.CompletedTask;
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

            var packet = packetFactory.CreateClosePacket();

            try
            {
                SessionContext.TransportHandler.Send(_byteSerializer.Serialize(packet));
            }
            catch (Exception exception)
            {
                _logger.LogErrorException(exception);
                SessionContext = null;
            }

            try
            {
                SessionContext.TransportHandler.Close();
            }
            catch (Exception exception)
            {
                _logger.LogErrorException(exception);
                SessionContext = null;
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
