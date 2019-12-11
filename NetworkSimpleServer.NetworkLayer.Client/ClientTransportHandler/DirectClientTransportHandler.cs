using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;

namespace NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler
{
    public class DirectClientTransportHandler : IClientTransportHandler
    {
        private readonly ITransportHandler _transportHandler;

        public DirectClientTransportHandler(ITransportHandler transportHandler)
        {
            _transportHandler = transportHandler;
        }

        public void Activate(TransportHandlerContext context)
        {
            _transportHandler.Activate(context);
        }

        public Packet AcceptedSend(Packet packet)
        {
            _transportHandler.Send(packet);

            return _transportHandler.Receive();
        }

        public Packet AcceptedSend(Packet packet, int packetSize)
        {
            _transportHandler.Send(packet);

            return _transportHandler.ReceiveSpecifiedSize(packetSize);
        }

        public void UnAcceptedSend(Packet packet)
        {
            _transportHandler.Send(packet);
        }

        public void ClearReceiveBuffer()
        {
            _transportHandler.ClearReceiveBuffer();
        }

        public void Close()
        {
            _transportHandler.Close();
        }

        public void Reconnect()
        {
            _transportHandler.Reconnect();
        }

        public void Dispose()
        {
            _transportHandler.Dispose();
        }
    }
}
