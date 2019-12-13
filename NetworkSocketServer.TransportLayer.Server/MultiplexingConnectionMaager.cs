using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSimpleServer.NetworkLayer.Server;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;
using NetworkSocketServer.TransportLayer.Core.Serializer;
using NetworkSocketServer.TransportLayer.PacketHandler;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler;

namespace NetworkSocketServer.TransportLayer.Server
{
    public class MultiplexingConnectionManager : IServiceConnectionManager
    {
        private readonly IRequestHandlerFactory _requestHandlerFactory;
        private readonly IList<ITransportHandler> _transportHandlers;
        private readonly IList<ServerSessionContext> _serverSessionContexts;

        public MultiplexingConnectionManager(IRequestHandlerFactory requestHandlerFactory)
        {
            _requestHandlerFactory = requestHandlerFactory;

            _transportHandlers = new List<ITransportHandler>();
            _serverSessionContexts = new List<ServerSessionContext>();
        }

        public async Task HandleNewConnection(ITransportHandler transportHandler)
        {
            _transportHandlers.Add(transportHandler);

            try
            {
                if (transportHandler.IsHaveNewPackets())
                {
                    await HandleNewPacket(transportHandler);
                }
            }
            catch
            {
                DeleteTransportHandler(transportHandler);
            }
        }

        public async Task ProcessRegistered()
        {
            foreach (var transportHandler in _transportHandlers)
            {
                try
                {
                    if (transportHandler.IsHaveNewPackets())
                    {
                        await HandleNewPacket(transportHandler);
                    }
                }
                catch(Exception exception)
                {
                    DeleteTransportHandler(transportHandler);
                }
            }
        }

        private async Task HandleNewPacket(ITransportHandler transportHandler)
        {
            var packet = transportHandler.Receive();

            var serverSessionContext = GetOrCreateContext(packet);

            var packetHandler = CreatePacketHandler(
                transportHandler,
                serverSessionContext);

            if (!await packetHandler.HandlePacket(packet))
            {
                DeleteTransportHandler(transportHandler);
                DeleteContext(serverSessionContext);
            }
        }

        private ServerSessionContext GetOrCreateContext(Packet packet)
        {
            foreach (var context in _serverSessionContexts)
            {
                if (context.SessionId == packet.SessionId)
                    return context;
            }

            var newContext = ServerSessionContext.CreateArrayBuffersContext(packet.SessionId);

            _serverSessionContexts.Add(newContext);

            return newContext;
        }

        private void DeleteTransportHandler(ITransportHandler transportHandler)
        {
            try
            {
                _transportHandlers.Remove(transportHandler);
            }
            catch
            {

            }
        }

        private void DeleteContext(ServerSessionContext context)
        {
            try
            {
                _serverSessionContexts.Remove(context);
            }
            catch
            {

            }
        }

        private IServerPacketHandler CreatePacketHandler(
            ITransportHandler transportHandler,
            ServerSessionContext serverSessionContext)
        {
            return new ServerPredictBasedServerPacketHandler(
                _requestHandlerFactory,
                transportHandler,
                new PacketFactory(serverSessionContext.SessionId),
                new BinaryFormatterBytesSerializer(),
                new ConsoleLogger(),
                serverSessionContext);
        }
    }
}
