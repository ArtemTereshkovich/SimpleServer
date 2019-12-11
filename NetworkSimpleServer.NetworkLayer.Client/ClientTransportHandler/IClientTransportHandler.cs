using System;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;

namespace NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler
{
    public interface IClientTransportHandler : IDisposable
    {
        void Activate(TransportHandlerContext context);

        Packet AcceptedSend(Packet packet);

        void UnAcceptedSend(Packet packet);

        void ClearReceiveBuffer();

        void Close();

        void Reconnect();
    }
}
