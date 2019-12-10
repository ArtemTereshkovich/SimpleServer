﻿using System;
using System.Net;
using System.Net.Sockets;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.Packets.Serializer;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Context;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Tcp;

namespace NetworkSimpleServer.NetworkLayer.Core.TransportHandler
{
    public class TcpBlockingReceiveTransportHandler : ITransportHandler
    {
        private readonly IPacketSerializer _packetSerializer;
        private readonly int _packetSize;

        private TcpTransportHandlerContext _context;

        public TcpBlockingReceiveTransportHandler(
            IPacketSerializer packetSerializer, 
            int packetSize)
        {
            _packetSerializer = packetSerializer;
            _packetSize = packetSize;
        }
        
        public void Dispose()
        {
            if (_context.AcceptedSocket?.Connected ?? false)
                _context.AcceptedSocket.Close();

            _context.AcceptedSocket?.Dispose();
        }

        public void Activate(TransportHandlerContext context)
        {
            _context = context as TcpTransportHandlerContext;
        }

        public void Send(Packet packet)
        {
            var array = _packetSerializer.Serialize(packet);

            if (array == null || array.Length != _packetSize)
                throw new ArgumentException(nameof(array));

            if (_context.AcceptedSocket == null)
                throw new InvalidOperationException(nameof(_context.AcceptedSocket));

            _context.AcceptedSocket.Send(array);
        }

        public void ClearReceiveBuffer()
        {
            var buffer = new byte[_context.AcceptedSocket.Available];
            _context.AcceptedSocket.Receive(buffer);
        }

        public Packet Receive()
        {
            var checkBuffer = new byte[0];
            _context.AcceptedSocket.Receive(checkBuffer);

            var buffer = new byte[_packetSize];
            _context.AcceptedSocket.Receive(buffer);

            return _packetSerializer.Deserialize(buffer);
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
            if (_context.AcceptedSocket.Connected)
            {
                ClearReceiveBuffer();

                _context.AcceptedSocket.Close();
            }

            _context.AcceptedSocket
                .Connect(_context.RemoteEndPoint);
        }
    }
}