using System;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.NetworkLayer.Server;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;
using NetworkSocketServer.TransportLayer.Core.Serializer;
using NetworkSocketServer.TransportLayer.PacketHandler;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;
using NetworkSocketServer.TransportLayer.Server.ServerPacketHandler;

namespace NetworkSocketServer.TransportLayer.Server
{
    public class SingleSessionConnectionManager : IServiceConnectionManager
    {
        private readonly IRequestHandlerFactory _requestHandlerFactory;
        private ServerSessionContext _serverSessionContext;

        public SingleSessionConnectionManager(IRequestHandlerFactory requestHandlerFactory)
        {
            _requestHandlerFactory = requestHandlerFactory;
        }

        public async Task HandleNewConnection(ITransportHandler transportHandler)
        {
            var packet = transportHandler.Receive();

            CheckContext(packet);

            var packetHandler = CreatePacketHandler(
                transportHandler,
                _serverSessionContext);

            if (await packetHandler.HandlePacket(packet))
            {

                await ContinueProcessingPacket(packetHandler, transportHandler);
            }
            else
            {
                _serverSessionContext = null;

                Console.WriteLine("Connection disconnected");
            }
        }

        private async Task ContinueProcessingPacket(
            IServerPacketHandler serverPacketHandler,
            ITransportHandler transportHandler)
        {
            while (true)
            {
                var packet = transportHandler.Receive();

                if (!await serverPacketHandler.HandlePacket(packet))
                {
                    _serverSessionContext = null;

                    Console.WriteLine("Connection disconnected");

                    return;
                }
            }
        }

        private void CheckContext(Packet packet)
        {
            if (_serverSessionContext != null
                && _serverSessionContext.SessionId == packet.SessionId)
            {
                return;
            }

            _serverSessionContext =
                ServerSessionContext.CreateArrayBuffersContext(packet.SessionId);

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