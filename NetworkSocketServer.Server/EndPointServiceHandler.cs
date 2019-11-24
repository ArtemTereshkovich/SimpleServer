using System;
using System.Threading.Tasks;
using NetworkSocketServer.Commands;
using NetworkSocketServer.NetworkLayer;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.Server.CommandHandlers;

namespace NetworkSocketServer.Server
{
    class EndPointServiceHandler : INewTransportHandler
    {
        public async Task HandleNewConnection(ITransportHandler transportHandler)
        {
            await (new SimpleServiceHandler(transportHandler)).StartReceive();
        }
    }

    internal class SimpleServiceHandler
    {
        private readonly ITransportHandler _transportHandler;
        private readonly FileDownloadCommandHandler _fileDownloadCommandHandler;
        private readonly UploadFileCommandHandler _uploadFileCommandHandler;
        private readonly TimeCommandHandler _timeCommandHandler;
        private readonly EchoCommandHandler _echoCommandHandler;

        public SimpleServiceHandler(ITransportHandler transportHandler)
        {
            _transportHandler = transportHandler;
            _fileDownloadCommandHandler = new FileDownloadCommandHandler(_transportHandler);
            _uploadFileCommandHandler = new UploadFileCommandHandler(_transportHandler);
            _timeCommandHandler = new TimeCommandHandler(_transportHandler);
            _echoCommandHandler = new EchoCommandHandler(_transportHandler);
        }

        public async Task StartReceive()
        {
            while (true)
            {
                var requestBytes = await _transportHandler.Receive();
            
                var request = requestBytes.Deserialize<Command>();

                Console.WriteLine($"Message received: {request.CommandType}");

                switch (request.CommandType)
                {
                    case CommandType.UploadFileRequest:
                        await _uploadFileCommandHandler.Handle(request);
                        break;
                    case CommandType.DownloadFileRequest:
                        await _fileDownloadCommandHandler.Handle(request);
                        break;
                    case CommandType.EchoRequest:
                        await _echoCommandHandler.Handle(request);
                        break;
                    case CommandType.TimeRequest:
                        await _timeCommandHandler.Handle(request);
                        break;
                    case CommandType.Disconnect:
                        _transportHandler.Close();
                        return ;
                    default:
                        continue;
                }
            }
        }
    }
}
