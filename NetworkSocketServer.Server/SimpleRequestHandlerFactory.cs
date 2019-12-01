using System;
using System.Collections.Generic;
using System.Text;
using NetworkSocketServer.TransportLayer;
using NetworkSocketServer.TransportLayer.Client.ServiceHandlers;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;
using NetworkSocketServer.TransportLayer.ServiceHandlers;

namespace NetworkSocketServer.Server
{
    class SimpleRequestHandlerFactory : IRequestHandlerFactory
    {
        public IRequestHandler CreateRequestHandler()
        {
            return new SimpleRequestHandler();
        }
    }
}
