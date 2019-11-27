using System;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.NetworkLayer.Connectors;
using NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher;
using NetworkSocketServer.TransportLayer.DTO;
using NetworkSocketServer.TransportLayer.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.ServiceHandlers.RequestExecutor;

namespace NetworkSocketServer.TransportLayer.ServiceHandlers.NetworkClientManager
{
    public class NetworkClientManager : INetworkClientManager
    {
        private readonly IByteSerializer _byteSerializer;
        private readonly IConnectorDispatcher _dispatcher;
        private readonly IRequestExecutorFactory _requestExecutorFactory;
        public ClientSessionContext SessionContext { get; private set; }

        public NetworkClientManager(IConnectorDispatcher dispatcher,
            IRequestExecutorFactory requestExecutorFactory)
        {
            _byteSerializer = new BinaryFormatterByteSerializer();;
            _dispatcher = dispatcher;
            _requestExecutorFactory = requestExecutorFactory;
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

                Console.WriteLine($"Succesfully connected at:{connectSettings.IpEndPointServer}");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                SessionContext = null;
            }
        }

        public async Task Reconnect()
        {
            if (SessionContext == null)
                throw new InvalidOperationException("No session context");

            try
            {
                SessionContext.TransportHandler.Close();
            }
            catch { }

            var transportHandler = await _dispatcher.CreateTransportHandler(
                SessionContext.CurrentConnectorSettings);

            SessionContext.ChangeTransportHandler(transportHandler);

            Console.WriteLine($"Succesfully reconnected at:{SessionContext.CurrentConnectorSettings.IpEndPointServer}");
        }

        public async Task<Response> HandleRequest(Request request)
        {
            if (SessionContext == null)
                throw new InvalidOperationException("No session context");

            try
            {
                var requestBytes = _byteSerializer.SerializeT(request);

                var requestExecutor = _requestExecutorFactory.Create(this);

                if (request is UploadFileRequest uploadFileRequest)
                {
                    requestBytes = uploadFileRequest.File;
                }

                var receiveBytes = await requestExecutor.HandleRequest(requestBytes);

                if (request is DownloadFileRequest downloadFileRequest)
                {
                    return new DownloadFileResponse
                    {
                        ResponseId = request.RequestId,
                        ErrorMessage = "",
                        File = receiveBytes,
                        Filename = "server.txt",
                        FileSize = receiveBytes.Length,
                        IsSuccess = true,
                    };
                }
                
                return _byteSerializer.DeserializeT<Response>(receiveBytes);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error" + exception.Message);

                SafeCloseSession();

                throw;
            }
        }

        public Task Disconnect()
        {
            if(SessionContext == null)
                throw new InvalidOperationException("No session context");


            SafeCloseSession();

            Console.Write("Successfuly disconnected");

            return Task.CompletedTask;
        }

        private void SafeCloseSession()
        {
            IPacketFactory packetFactory = new PacketFactory.PacketFactory(SessionContext.SessionId);

            var packet = packetFactory.CreateClosePacket();

            try
            {
                SessionContext.TransportHandler.Send(_byteSerializer.Serialize(packet));
            }
            catch{}

            try
            {
                SessionContext.TransportHandler.Close();
            }
            catch{}

            SessionContext = null;
        }
    }
}
