using System;
using System.Net;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Context;

namespace NetworkSimpleServer.NetworkLayer.Core.TransportHandler
{
    public interface ITransportHandler : IDisposable
    {
        void Activate(TransportHandlerContext context);

        void Send(Packet packet);

        void ClearReceiveBuffer();

        Packet Receive();

        void Close();

        void Reconnect();
    }
}
