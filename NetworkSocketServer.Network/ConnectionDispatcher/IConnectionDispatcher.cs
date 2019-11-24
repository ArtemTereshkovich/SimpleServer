namespace NetworkSocketServer.NetworkLayer.ConnectionDispatcher
{
    internal interface IConnectionDispatcher : INetworkAcceptorSubscriber
    {
        void StartListen();

        void StopListen();
    }
}
