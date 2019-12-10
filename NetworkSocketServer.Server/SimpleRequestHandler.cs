using System;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.Server.CommandHandlers;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;

namespace NetworkSocketServer.Server
{
    internal class SimpleRequestHandler : IRequestHandler
    {
        private readonly DownloadFileRequestHandler _downloadFileRequestHandler;
        private readonly UploadFileRequestHandler _uploadFileRequestHandler;
        private readonly DateRequestHandler _dateRequestHandler;
        private readonly TextRequestHandler _textRequestHandler;

        public SimpleRequestHandler()
        {
            _downloadFileRequestHandler = new DownloadFileRequestHandler();
            _uploadFileRequestHandler = new UploadFileRequestHandler();
            _dateRequestHandler = new DateRequestHandler();
            _textRequestHandler = new TextRequestHandler();
        }

        public async Task<Response> HandleRequest(Request request)
        {
            Console.WriteLine($"Message received: {request.RequestId}");

            switch (request)
            {
                case UploadFileRequest fileRequest:
                    return await _uploadFileRequestHandler.Handle(fileRequest);
                case DownloadFileRequest downloadFileRequest:
                    return await _downloadFileRequestHandler.Handle(request);
                case TextRequest textRequest:
                    return await _textRequestHandler.Handle(request);
                case DateRequest dateRequest:
                    return await _dateRequestHandler.Handle(request);

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
