﻿using System;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.PacketHandler;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.ServiceHandlers;

namespace NetworkSocketServer.TransportLayer.Server
{
    public class SingleSessionConnectionManager : IConnectionManager
    {
        private readonly IRequestHandlerFactory _requestHandlerFactory;
        private ServerSessionContext _serverSessionContext;
        private readonly IByteSerializer _byteSerializer;
        private Guid _sessionId = Guid.Empty;

        public SingleSessionConnectionManager(IRequestHandlerFactory requestHandlerFactory)
        {
            _requestHandlerFactory = requestHandlerFactory;
            _byteSerializer = new BinaryFormatterByteSerializer();
        }

        public async Task HandleNewConnection(ITransportHandler transportHandler)
        {
            var packet = ReceivePacketMessage(transportHandler);

            CheckContext(packet);

            var packetHandler = CreatePacketHandler(
                transportHandler, _serverSessionContext, packet.SessionId);

            if (await packetHandler.HandlePacket(packet))
            {

                await ContinueProcessingPacket(packetHandler, transportHandler);
            }
            else
            {
                _serverSessionContext = null;
                _sessionId = Guid.Empty;

                Console.WriteLine("Connection disconnected");
            }
        }

        private async Task ContinueProcessingPacket(
            IServerPacketHandler serverPacketHandler,ITransportHandler transportHandler)
        {
            while (true)
            {
                var packet = ReceivePacketMessage(transportHandler);

                if (!await serverPacketHandler.HandlePacket(packet))
                {
                    _serverSessionContext = null;
                    _sessionId = Guid.Empty;

                    Console.WriteLine("Connection disconnected");

                    return;
                }
            }
        }

        private void CheckContext(Packet packet)
        {
            if (_serverSessionContext != null && _sessionId == packet.SessionId) return;

            _serverSessionContext = ServerSessionContext.CreateArrayBuffersContext();

            _sessionId = packet.SessionId;
        }

        private Packet ReceivePacketMessage(ITransportHandler transportHandler)
        {
            var messageBytes = transportHandler.Receive();

            return _byteSerializer.Deserialize(messageBytes);
        }

        private IServerPacketHandler CreatePacketHandler(
            ITransportHandler transportHandler,
            ServerSessionContext serverSessionContext,
            Guid connectionId)
        {
            return new ServerPredictBasedServerPacketHandler(
                connectionId,
                _requestHandlerFactory,
                transportHandler,
                serverSessionContext);
        }
    }
}