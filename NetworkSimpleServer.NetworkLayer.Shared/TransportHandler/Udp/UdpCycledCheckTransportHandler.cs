using System;
using System.Net.Sockets;
using System.Threading;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.Packets.Formatter;

namespace NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Udp
{
    public class UdpCycledCheckTransportHandler : ITransportHandler
    {
        private readonly IPacketByteFormatter _packetByteFormatter;
        private readonly int _packetSize;

        private UdpTransportHandlerContext _context;

        public UdpCycledCheckTransportHandler(
            IPacketByteFormatter packetByteFormatter,
            int packetSize)
        {
            _packetSize = packetSize;
            _packetByteFormatter = packetByteFormatter;
        }

        public void Dispose()
        {
            if (_context.AcceptedSocket?.Connected ?? false)
                _context.AcceptedSocket.Close();

            _context.AcceptedSocket?.Dispose();
        }

        public bool IsHaveNewPackets()
        {
            return _context.AcceptedSocket.Available != 0;
        }

        public void Activate(TransportHandlerContext context)
        {
            _context = context as UdpTransportHandlerContext;

            _context.AcceptedSocket = new Socket(
                _context.RemoteEndPoint.AddressFamily,
                SocketType.Dgram,
                ProtocolType.Udp);

            _context.AcceptedSocket
                .Connect(_context.RemoteEndPoint);
        }

        public void Send(Packet packet)
        {
            var array = _packetByteFormatter.Serialize(packet);

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
            //WaitForData(_packetSize);

            var remoteEndpoint = _context.RemoteEndPoint;

            var checkArray = new byte[0];
            _context.AcceptedSocket.ReceiveFrom(checkArray, ref remoteEndpoint);

            if (_context.AcceptedSocket.Available < _packetSize)
                Thread.Sleep(5);

            if (_context.AcceptedSocket.Available < _packetSize)
                Thread.Sleep(10);

            if (_context.AcceptedSocket.Available < _packetSize)
                Thread.Sleep(15);

            if (_context.AcceptedSocket.Available < _packetSize)
                Thread.Sleep(50);

            var array = new byte[_packetSize];
            _context.AcceptedSocket.ReceiveFrom(array, ref remoteEndpoint);

            return _packetByteFormatter.Deserialize(array);
        }

        public Packet ReceiveSpecifiedSize(int specifiedPacketSize)
        {
            //WaitForData(specifiedPacketSize);
            
            var remoteEndpoint = _context.RemoteEndPoint;

            var checkArray = new byte[0];
            _context.AcceptedSocket.ReceiveFrom(checkArray, ref remoteEndpoint);

            if (_context.AcceptedSocket.Available < specifiedPacketSize)
                Thread.Sleep(5);

            if (_context.AcceptedSocket.Available < specifiedPacketSize)
                Thread.Sleep(10);

            if (_context.AcceptedSocket.Available < specifiedPacketSize)
                Thread.Sleep(15);

            if (_context.AcceptedSocket.Available < specifiedPacketSize)
                Thread.Sleep(50);

            var array = new byte[specifiedPacketSize];
            _context.AcceptedSocket.ReceiveFrom(array, ref remoteEndpoint);

            return _packetByteFormatter.Deserialize(array);
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
                _context.AcceptedSocket.Close();
            }

            try
            {
                ClearReceiveBuffer();
            }
            catch { }

            try
            {
                _context.AcceptedSocket
                    .Connect(_context.RemoteEndPoint);
            }
            catch (ObjectDisposedException)
            {
                _context.AcceptedSocket = new Socket(
                    _context.RemoteEndPoint.AddressFamily,
                    SocketType.Dgram,
                    ProtocolType.Udp);

                _context.AcceptedSocket
                    .Connect(_context.RemoteEndPoint);
            }
        }

        private void WaitForData(int packetSize)
        {
            while (true)
            {
                System.Threading.Thread.Sleep(10);

                if (_context.AcceptedSocket.Available == packetSize)
                    return;
            }
        }
    }
}
