using System;
using NetworkSocketServer.Server.ServiceHandlers;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;

namespace NetworkSocketServer.Server
{
    class IdBasedRequestHandlerFactory : IRequestHandlerFactory
    {
        public IRequestHandler CreateRequestHandler(int serviceId)
        {
            switch (serviceId)
            {
                case 1:
                    return new DateServiceHandler();
                case 2:
                    return new TextServiceHandler();
                case 3:
                    return new DownloadRequestHandler();
                case 4:
                    return new UploadServiceHandler();
                default:
                    throw new ArgumentException("Unsupported Service Id");
            }
        }
    }
}
