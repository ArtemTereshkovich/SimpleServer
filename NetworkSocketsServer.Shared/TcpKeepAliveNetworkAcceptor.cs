using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkSocketsServer.Shared
{
    public class TcpKeepAliveNetworkAcceptor : INetworkAcceptor
    {
        private readonly TcpListener _acceptorListener;

        private readonly TcpNetworkAcceptorSettings _acceptorSettings;

        public TcpKeepAliveNetworkAcceptor(TcpNetworkAcceptorSettings acceptorSettings)
        {

            _acceptorListener = new TcpListener(
                acceptorSettings.ListenIpAddress,
                acceptorSettings.ListenPort);

            _acceptorSettings = acceptorSettings;
        }

        public async Task<INetworkConnection> AcceptConnection()
        {
            var acceptedSocket = await _acceptorListener.AcceptSocketAsync();

            return TcpKeepAliveNetworkConnection.CreateConnectionFromSocket(acceptedSocket);
        }

        public bool IsHaveNewConnection()
        {
            return _acceptorListener.Pending();
        }

        public void StartListen()
        {
            _acceptorListener.Start(_acceptorSettings.ListenMaxBacklogConnection);
        }

        public void StopListen()
        {
            _acceptorListener.Stop();
        }
    }
}