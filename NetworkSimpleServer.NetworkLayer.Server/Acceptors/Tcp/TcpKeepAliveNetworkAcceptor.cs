using System.Net.Sockets;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.SocketOptionsAccessor;
using NetworkSimpleServer.NetworkLayer.Server.Acceptors;
using NetworkSocketServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.NetworkLayer.Core.TransportHandler.Tcp;

namespace NetworkSocketServer.NetworkLayer.Server.Acceptors.Tcp
{
    internal class TcpKeepAliveNetworkAcceptor : INetworkAcceptor
    {
        private readonly TcpListener _acceptorListener;

        private readonly TcpNetworkAcceptorSettings _acceptorSettings;
        private readonly ISocketOptionsAccessorFactory _socketOptionsAccessorFactory;

        public TcpKeepAliveNetworkAcceptor(
            TcpNetworkAcceptorSettings acceptorSettings, 
            ISocketOptionsAccessorFactory socketOptionsAccessorFactory)
        {

            _acceptorListener = new TcpListener(
                acceptorSettings.ListenIpAddress,
                acceptorSettings.ListenPort);

            _acceptorSettings = acceptorSettings;
            _socketOptionsAccessorFactory = socketOptionsAccessorFactory;
        }

        public async Task AcceptConnection(ITransportHandler networkTransportHandler)
        {
            var acceptedSocket = await _acceptorListener.AcceptSocketAsync();

            _socketOptionsAccessorFactory
                .GetSocketOptionsAccessor()
                .SetOptions(acceptedSocket);

            networkTransportHandler.Activate(new TcpTransportHandlerContext
            {
                AcceptedSocket = acceptedSocket,
                RemoteEndPoint = acceptedSocket.RemoteEndPoint,
                ServiceId = _acceptorSettings.ServiceId,
            });
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
