using System;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.NetworkLayer.Connectors;
using NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher;
using NetworkSocketServer.TransportLayer.DTO;
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

                sessionContext = ClientSessionContext.CreateClientContext(transportHandler,  Guid.NewGuid());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                sessionContext = null;
            }
        }

        public Task<Response> HandleRequest(Request request)
        {
            var requestBytes = _byteSerializer.Serialize(request);

            var packetFactory = new PacketFactory.PacketFactory(sessionContext.SessionId);

            var packet = packetFactory.CreateExecutePayload(requestBytes);

            sessionContext.TransportHandler.Send(_byteSerializer.Serialize(packet));

            var receiveBytes = sessionContext.TransportHandler.Receive();

            var packetResponse = _byteSerializer.Deserialize<Packet>(receiveBytes);

            return Task.FromResult(_byteSerializer.Deserialize<Response>(packetResponse.Payload));
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
