using System;
using System.Net;
using NetworkSimpleServer.NetworkLayer.Core.Packets;

namespace NetworkSimpleServer.NetworkLayer.Core.TransportHandler
{
    public interface ITransportHandler : IDisposable
    {
        void Activate(TransportHandlerContext context);

        void Send(Packet packet);

        void ClearReceiveBuffer();

        Packet Receive();

        Packet ReceiveSpecifiedSize(int packetSize);

        void Close();

        void Reconnect();
    }
}
