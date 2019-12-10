namespace NetworkSimpleServer.NetworkLayer.Server.AcceptorDispatcher
{
    internal interface IAcceptorDispatcher : INetworkAcceptorSubscriber
    {
        void StartListen();

        void StopListen();
    }
}
