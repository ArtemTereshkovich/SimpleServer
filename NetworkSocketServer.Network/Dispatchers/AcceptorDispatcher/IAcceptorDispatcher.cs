namespace NetworkSocketServer.NetworkLayer.Dispatchers.AcceptorDispatcher
{
    internal interface IAcceptorDispatcher : INetworkAcceptorSubscriber
    {
        void StartListen();

        void StopListen();
    }
}
