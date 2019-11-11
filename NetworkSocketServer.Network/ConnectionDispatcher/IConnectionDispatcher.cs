namespace NetworkSocketServer.Network.ConnectionDispatcher
{
    internal interface IConnectionDispatcher
    {
        void StartListen();

        void StopListen();
    }
}
