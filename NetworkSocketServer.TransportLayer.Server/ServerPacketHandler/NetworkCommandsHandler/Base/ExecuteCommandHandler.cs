using System;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base
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

        public async Task<bool> Handle(Packet clientPacket)
        {
            var resultBytes = await HandleCommand(clientPacket);

            CleanUpBuffers();

            return SendResult(resultBytes, clientPacket);
        }

        protected abstract Task<byte[]> HandleCommand(Packet packet);

        private bool SendResult(byte[] resultExecution, Packet clientPacket)
        {
            if (resultExecution.Length <= ServerSessionContext.PacketPayloadThreshold)
            {
                var resultExecutionLength = resultExecution.Length;

                Array.Resize(ref resultExecution, ServerSessionContext.PacketPayloadThreshold);

                var answerPacket = _packetFactory
                    .CreateAnswerExecuteSuccessPayload(
                        clientPacket.PacketId, 
                        resultExecution, 
                        resultExecutionLength);

                SendPacket(answerPacket);
            }
            else
            {
                UpdateTransmitBuffer(resultExecution);

                var answerPacket = _packetFactory
                    .CreateAnswerExecuteSuccessBuffer(
                        clientPacket.PacketId, 
                        ServerSessionContext.TransmitBuffer.Length);

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
