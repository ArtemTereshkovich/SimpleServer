using System;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading.Tasks;
using NetworkSocketServer.Network.Tcp.KeepAlive;
using NetworkSocketServer.Network.TransportHandler;

namespace NetworkSocketServer.Network.Tcp
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
            try
            {
                var acceptedSocket = await _acceptorListener.AcceptSocketAsync();

                _socketOptionsAccessorFactory
                    .GetSocketOptionsAccessor()
                    .SetOptions(acceptedSocket);

                networkTransportHandler.Activate(acceptedSocket);
            }
            catch (Exception exception)
            {
                throw;
            }
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
