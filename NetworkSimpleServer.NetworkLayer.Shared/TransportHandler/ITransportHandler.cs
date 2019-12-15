using System;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;

namespace NetworkSocketServer.NetworkLayer.Core.TransportHandler
{
    public interface ITransportHandler : IDisposable
    {
        int ServiceId { get; }

        bool IsHaveNewPackets();

        void Activate(TransportHandlerContext context);

        void Send(Packet packet);

        void ClearReceiveBuffer();

        Packet Receive();

        Packet ReceiveSpecifiedSize(int packetSize);

        void Close();

        void Reconnect();
    }
}
