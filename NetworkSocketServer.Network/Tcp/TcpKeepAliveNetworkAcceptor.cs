using System.Net.Sockets;
using System.Threading.Tasks;
using NetworkSocketServer.Network.Tcp.KeepAlive;

namespace NetworkSocketServer.Network.Tcp
{
    internal class TcpKeepAliveNetworkAcceptor : INetworkAcceptor
    {
        private readonly TcpListener _acceptorListener;

        private readonly TcpNetworkAcceptorSettings _acceptorSettings;
        private readonly SocketKeepAliveOptions _socketKeepAliveOptions;
        private readonly ISocketOptionsAccessor _socketOptionsAccessor;

        public TcpKeepAliveNetworkAcceptor(
            TcpNetworkAcceptorSettings acceptorSettings, 
            SocketKeepAliveOptions socketKeepAliveOptions,
            ISocketOptionsAccessor socketOptionsAccessor)
        {

            _acceptorListener = new TcpListener(
                acceptorSettings.ListenIpAddress,
                acceptorSettings.ListenPort);

            _acceptorSettings = acceptorSettings;
            _socketKeepAliveOptions = socketKeepAliveOptions;
            _socketOptionsAccessor = socketOptionsAccessor;
        }

        public async Task AcceptConnection(INetworkServiceHandle networkServiceHandle)
        {
            var acceptedSocket = await _acceptorListener.AcceptSocketAsync();

            _socketOptionsAccessor.SetKeepAliveOptions(acceptedSocket, _socketKeepAliveOptions);
        }

        public bool IsHaveNewConnection()
        {
            return _acceptorListener.Pending();
        }

        public void Open()
        {
            _acceptorListener.Start(_acceptorSettings.ListenMaxBacklogConnection);
        }

        public void Close()
        {
            _acceptorListener.Stop();
        }
    }
}
