﻿using System;
using System.Net.Sockets;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.Packets.Serializer;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Context;

namespace NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Udp
{
    public class UdpCycledCheckTransportHandler : ITransportHandler
    {
        private readonly IPacketSerializer _packetSerializer;
        private readonly int _packetSize;

        private UdpTransportHandlerContext _context;

        public UdpCycledCheckTransportHandler(
            IPacketSerializer packetSerializer,
            int packetSize)
        {
            _packetSize = packetSize;
            _packetSerializer = packetSerializer;
        }

        public void Dispose()
        {
            if (_context.AcceptedSocket?.Connected ?? false)
                _context.AcceptedSocket.Close();

            _context.AcceptedSocket?.Dispose();
        }

        public void Activate(TransportHandlerContext context)
        {
            _context = context as UdpTransportHandlerContext;
        }

        public void Send(Packet packet)
        {
            var array = _packetSerializer.Serialize(packet);

            if(array == null || array.Length != _packetSize)
                throw new ArgumentException(nameof(array));

            if (_context.AcceptedSocket == null || !_context.AcceptedSocket.Connected)
                throw new InvalidOperationException(nameof(_context.AcceptedSocket));

            _context.AcceptedSocket.SendTo(array, _context.RemoteEndPoint);
        }

        public void ClearReceiveBuffer()
        {
            var remoteEndpoint = _context.RemoteEndPoint;
            var buffer = new byte[_context.AcceptedSocket.Available];

            _context.AcceptedSocket.ReceiveFrom(buffer, ref remoteEndpoint);
        }

        public Packet Receive()
        {
            WaitForData();

            var remoteEndpoint = _context.RemoteEndPoint;
            var array = new byte[_packetSize];
            _context.AcceptedSocket.ReceiveFrom(array, ref remoteEndpoint);

            return _packetSerializer.Deserialize(array);
        }

        public void Close()
        {
            if (_context.AcceptedSocket == null || !_context.AcceptedSocket.Connected)
                throw new InvalidOperationException(nameof(_context.AcceptedSocket));

            ClearReceiveBuffer();

            Dispose();
        }

        public void Reconnect()
        {
            try
            {
                ClearReceiveBuffer();
            }
            catch
            {

            }

            _context.AcceptedSocket = new Socket(
                _context.RemoteEndPoint.AddressFamily,
                SocketType.Dgram, 
                ProtocolType.Udp);

            _context.AcceptedSocket.Connect(_context.RemoteEndPoint);
        }

        private void WaitForData()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(10);

                if (_context.AcceptedSocket.Available >= _packetSize)
                    return;
            }
        }
    }
}