using System.Threading.Tasks;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.Packets;
using NetworkSocketServer.TransportLayer.Packets.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.Server;

namespace NetworkSocketServer.TransportLayer.PacketHandler.NetworkCommandsHandler.Base
{
    abstract class ExecuteCommandHandler : SenderCommandHandler, INetworkCommandHandler 
    {
        protected readonly ServerSessionContext ServerSessionContext;
        private readonly IPacketFactory _packetFactory;

        protected ExecuteCommandHandler(
            ServerSessionContext serverSessionContext,
            IPacketFactory packetFactory,
            IByteSerializer byteSerializer, 
            ITransportHandler transportHandler)
            :base(transportHandler, byteSerializer)
        {
            ServerSessionContext = serverSessionContext;
            _packetFactory = packetFactory;
        }

        public async Task<bool> Handle(Packet packet)
        {
            var resultBytes = await HandleCommand(packet);

            CleanUpBuffers();

            return SendResult(resultBytes);
        }

        protected abstract Task<byte[]> HandleCommand(Packet packet);

        private bool SendResult(byte[] resultExecution)
        {
            if (resultExecution.Length > ServerSessionContext.PacketPayloadThreshold * 2)
            {
                var answerPacket = _packetFactory
                    .CreateAnswerExecuteSuccessPayload(resultExecution, resultExecution.Length);

                SendPacket(answerPacket);
            }
            else
            {
                UpdateTransmitBuffer(resultExecution);

                var answerPacket = _packetFactory
                    .CreateAnswerExecuteSuccessBuffer(resultExecution.Length);

                SendPacket(answerPacket);
            }

            return true;
        }

        private void CleanUpBuffers()
        {
            ServerSessionContext.TransmitBuffer.Reinitialize(0);
            ServerSessionContext.ReceiveBuffer.Reinitialize(0);
        }

        private void UpdateTransmitBuffer(byte[] content)
        {
            ServerSessionContext.TransmitBuffer.Reinitialize(content.Length);
            ServerSessionContext.TransmitBuffer.Insert(content, 0);
        }
    }
}
