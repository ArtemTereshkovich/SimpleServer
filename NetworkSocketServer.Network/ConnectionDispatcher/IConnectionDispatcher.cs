namespace NetworkSocketServer.Network.ConnectionDispatcher
{
    internal interface IConnectionDispatcher : INetworkAcceptorSubscriber
    {
        void StartListen();

        void StopListen();
    }
}
