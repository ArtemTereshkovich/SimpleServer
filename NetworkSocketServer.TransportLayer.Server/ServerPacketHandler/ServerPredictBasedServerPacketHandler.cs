using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;
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
            Guid sessionId,
            IRequestHandlerFactory requestHandlerFactory,
            ITransportHandler transportHandler,
            ServerSessionContext serverSessionContext)
        {
            IPacketFactory packetFactory = new PacketFactory(sessionId);
            IByteSerializer byteSerializer = new BinaryFormatterByteSerializer();

            var requestHandlerFactory1 = requestHandlerFactory;
            var transportHandler1 = transportHandler;
            var serverSessionContext1 = serverSessionContext;

            _handlers = new Dictionary<PacketClientCommand, INetworkCommandHandler>
            {
                {
                    PacketClientCommand.Read,
                    new ReadCommandHandler(
                        serverSessionContext1, 
                        packetFactory, 
                        transportHandler1, 
                        byteSerializer)
                },
                {
                    PacketClientCommand.Write, 
                    new WriteCommandHandler(
                        serverSessionContext1, 
                        packetFactory, 
                        transportHandler1, 
                        byteSerializer)
                },
                {
                    PacketClientCommand.ExecuteBuffer, 
                    new ExecuteBufferCommandHandler(
                        serverSessionContext1,
                        requestHandlerFactory1,
                        packetFactory, 
                        byteSerializer, 
                        transportHandler1)
                },
                {
                    PacketClientCommand.ExecutePayload, 
                    new ExecutePayloadCommandHandler(
                        serverSessionContext1, 
                        requestHandlerFactory1, 
                        packetFactory, 
                        byteSerializer, 
                        transportHandler1)
                },
                {
                    PacketClientCommand.Close, 
                    new CloseCommandHandler(transportHandler1)
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
