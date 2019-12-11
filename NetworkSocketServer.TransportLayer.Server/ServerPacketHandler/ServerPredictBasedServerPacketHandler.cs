using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;
using NetworkSocketServer.TransportLayer.Core.Serializer;
using NetworkSocketServer.TransportLayer.Server;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base;

namespace NetworkSocketServer.TransportLayer.PacketHandler
{
    class ServerPredictBasedServerPacketHandler : IServerPacketHandler
    {
        private readonly IDictionary<PacketClientCommand, INetworkCommandHandler> _handlers;
        private readonly ITransportHandler _transportHandler;
 
        public ServerPredictBasedServerPacketHandler(
            IRequestHandlerFactory requestHandlerFactory,
            ITransportHandler transportHandler,
            IPacketFactory packetFactory,
            IBytesSerializer bytesSerializer,
            ILogger logger,
            ServerSessionContext serverSessionContext)
        {
            _transportHandler = transportHandler;

            _handlers = new Dictionary<PacketClientCommand, INetworkCommandHandler>
            {
                {
                    PacketClientCommand.Read,
                    new ReadCommandHandler(
                        logger,
                        serverSessionContext,
                        packetFactory,
                        _transportHandler)
                },
                {
                    PacketClientCommand.Write, 
                    new WriteCommandHandler(
                        serverSessionContext,
                        packetFactory,
                        _transportHandler,
                        logger)
                },
                {
                    PacketClientCommand.ExecuteBuffer, 
                    new ExecuteBufferCommandHandler(
                        serverSessionContext,
                        requestHandlerFactory,
                        packetFactory,
                        bytesSerializer,
                        _transportHandler)
                },
                {
                    PacketClientCommand.ExecutePayload, 
                    new ExecutePayloadCommandHandler(
                        serverSessionContext,
                        requestHandlerFactory,
                        packetFactory,
                        bytesSerializer,
                        _transportHandler)
                },
                {
                    PacketClientCommand.Close, 
                    new CloseCommandHandler(_transportHandler)
                }
            };
        }
       
        public async Task<bool> HandlePacket(Packet packet)
        {
             
            if (_handlers.TryGetValue(packet.PacketClientCommand, out var handler))
            {
                return await handler.Handle(packet);
            }

            _transportHandler.ClearReceiveBuffer();

            throw new InvalidOperationException("Receive unsupported command. Possible keep alive packet.");
        }
    }
}
