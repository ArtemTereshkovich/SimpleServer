using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;

namespace NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler.Decorators
{
    class ClientTransportHandlerWithPacketChecking : IClientTransportHandler
    {
        private readonly IClientTransportHandler _origin;

        public ClientTransportHandlerWithPacketChecking(IClientTransportHandler origin)
        {
            _origin = origin;
        }

        public void Dispose()
        {
            _origin.Dispose();
        }

        public void Activate(TransportHandlerContext context)
        {
            _origin.Activate(context);
        }

        public void ClearReceiveBuffer()
        {
            _origin.ClearReceiveBuffer();
        }
        
        public void Close()
        {
           _origin.Close();
        }

        public void Reconnect()
        {
            _origin.Reconnect();
        }

        public Packet AcceptedSend(Packet packet)
        {
            var answerPacket = _origin.AcceptedSend(packet);

            if(answerPacket.PacketId != packet.PacketId)
                throw new UnacceptedPacketException("Receive unaccpted packet:" + answerPacket.PacketId);

            return answerPacket;
        }
    }
}
