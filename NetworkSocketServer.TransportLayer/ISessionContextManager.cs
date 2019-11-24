using System;

namespace NetworkSocketServer.TransportLayer
{
    interface ISessionContextManager
    {
        void ClearContext(Guid clientId);
    }
}
