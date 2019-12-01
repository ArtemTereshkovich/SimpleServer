using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.PacketHandler.NetworkCommandsHandler;
using NetworkSocketServer.TransportLayer.PacketHandler.NetworkCommandsHandler.Base;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.Server;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler;
using NetworkSocketServer.TransportLayer.ServiceHandlers;

namespace NetworkSocketServer.TransportLayer.PacketHandler
{
    class ServerPredictBasedServerPacketHandler : IServerPacketHandler
    {
        private readonly IRequestHandlerFactory _requestHandlerFactory;
        private readonly ITransportHandler _transportHandler;
        private readonly ServerSessionContext _serverSessionContext;
        private readonly IPacketFactory _packetFactory;
        private readonly IByteSerializer _byteSerializer;

        private readonly IDictionary<PacketClientCommand, INetworkCommandHandler> _handlers;
 
        public ServerPredictBasedServerPacketHandler(
            Guid sessionId,
            IRequestHandlerFactory requestHandlerFactory,
            ITransportHandler transportHandler,
            ServerSessionContext serverSessionContext)
        {
            _packetFactory = new Packets.PacketFactory.PacketFactory(sessionId);
            _byteSerializer = new BinaryFormatterByteSerializer();

            _requestHandlerFactory = requestHandlerFactory;
            _transportHandler = transportHandler;
            _serverSessionContext = serverSessionContext;

            _handlers = new Dictionary<PacketClientCommand, INetworkCommandHandler>
            {
                {
                    PacketClientCommand.Read,
                    new ReadCommandHandler(
                        _serverSessionContext, 
                        _packetFactory, 
                        _transportHandler, 
                        _byteSerializer)
                },
                {
                    PacketClientCommand.Write, 
                    new WriteCommandHandler(
                        _serverSessionContext, 
                        _packetFactory, 
                        _transportHandler, 
                        _byteSerializer)
                },
                {
                    PacketClientCommand.ExecuteBuffer, 
                    new ExecuteBufferCommandHandler(
                        _serverSessionContext,
                        _requestHandlerFactory,
                        _packetFactory, 
                        _byteSerializer, 
                        _transportHandler)
                },
                {
                    PacketClientCommand.ExecutePayload, 
                    new ExecutePayloadCommandHandler(
                        _serverSessionContext, 
                        _requestHandlerFactory, 
                        _packetFactory, 
                        _byteSerializer, 
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

            Console.WriteLine("Receive unsupported command. Possible keep alive packet.");
            return true;
        }
    }
}
