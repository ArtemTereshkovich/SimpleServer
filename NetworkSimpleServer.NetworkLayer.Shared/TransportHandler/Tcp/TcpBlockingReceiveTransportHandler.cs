using System;
using System.Net.Sockets;
using System.Threading;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.Packets.Formatter;

namespace NetworkSimpleServer.NetworkLayer.Core.TransportHandler.Tcp
{
    public class TcpBlockingReceiveTransportHandler : ITransportHandler
    {
        private readonly IPacketByteFormatter _packetByteFormatter;
        private readonly int _packetSize;

        private TcpTransportHandlerContext _context;

        public TcpBlockingReceiveTransportHandler(
            IPacketByteFormatter packetByteFormatter, 
            int packetSize)
        {
            _packetByteFormatter = packetByteFormatter;
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
            var array = _packetByteFormatter.Serialize(packet);

            if (array == null || array.Length < _packetSize)
                throw new ArgumentException(nameof(array));

            if (_context.AcceptedSocket == null)
                throw new InvalidOperationException(nameof(_context.AcceptedSocket));

            _context.AcceptedSocket.Send(array);
        }

        public void ClearReceiveBuffer()
        {
            if (_context.AcceptedSocket.Available != 0)
            {
                var buffer = new byte[_context.AcceptedSocket.Available];
                _context.AcceptedSocket.Receive(buffer);
            }
        }

        public Packet Receive()
        {
            var checkBuffer = new byte[0];
            _context.AcceptedSocket.Receive(checkBuffer);

            if (_context.AcceptedSocket.Available < _packetSize)
                Thread.Sleep(15);

            var buffer = new byte[_packetSize];
            _context.AcceptedSocket.Receive(buffer);

            return _packetByteFormatter.Deserialize(buffer);
        }

        public Packet ReceiveSpecifiedSize(int specifiedPacketSize)
        {
            var checkBuffer = new byte[0];
            _context.AcceptedSocket.Receive(checkBuffer);
            
            Thread.Sleep(40);

            if (_context.AcceptedSocket.Available < specifiedPacketSize)
                Thread.Sleep(15);

            var buffer = new byte[specifiedPacketSize];
            _context.AcceptedSocket.Receive(buffer);

            return _packetByteFormatter.Deserialize(buffer);
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
                    SocketType.Stream,
                    ProtocolType.Tcp);

                _context.AcceptedSocket
                    .Connect(_context.RemoteEndPoint);
            }
        }
    }
}
