using System;
using System.Threading.Tasks;
using NetworkSocketServer.Network.TransportHandler;

namespace NetworkSocketServer.Network.ThreadSet
{
    class StubThreadSet : IThreadSet
    {

        public void Execute(INetworkServiceHandler serviceHandler, Task<ITransportHandler> transportHandler)
        {
            try
            {
                transportHandler.Wait();
                serviceHandler.HandleNewConnection(transportHandler.Result).Wait();
            }
            catch (AggregateException exception)
            {
                throw;
            }
        }
    }
}
