using System;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.NetworkLayer.Connectors;
using NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher;
using NetworkSocketServer.TransportLayer.Serializer;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.NetworkRequestExecutor
{
    class NetworkRequestExecutor : INetworkRequestExecutor
    {
        private readonly IByteSerializer _byteSerializer;
        private readonly IConnectorDispatcher _dispatcher;
        private ClientSessionContext sessionContext;

        public NetworkRequestExecutor(IConnectorDispatcher dispatcher)
        {
            _byteSerializer = new BinaryFormatterByteSerializer();;
            _dispatcher = dispatcher;
        }

        public bool IsConnected => sessionContext != null;

        public async Task Connect(NetworkConnectorSettings connectSettings)
        {
            try
            {
                var transportHandler = await _dispatcher.CreateTransportHandler(connectSettings);

                sessionContext = ClientSessionContext.CreateClientContext(transportHandler);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                sessionContext = null;
            }
        }

        public Task<Response> HandleRequest(Request request)
        {
            throw new NotImplementedException();
        }

        public Task Disconnect()
        {
            if(sessionContext == null)
                throw new InvalidOperationException(nameof(sessionContext));


            var packetFactory = new PacketFactory.PacketFactory(sessionContext.SessionId);

            var packet = packetFactory.CreateClosePacket();

            sessionContext.TransportHandler.Send(_byteSerializer.Serialize(packet));

            sessionContext.TransportHandler.Close();

            sessionContext = null;
            
            return Task.CompletedTask;
        }
    }
}
