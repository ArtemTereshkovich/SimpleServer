using System;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base
{
    abstract class ExecuteCommandHandler : INetworkCommandHandler 
    {
        protected readonly ServerSessionContext ServerSessionContext;
        protected readonly ITransportHandler TransportHandler;

        private readonly IPacketFactory _packetFactory;

        protected ExecuteCommandHandler(
            ServerSessionContext serverSessionContext,
            IPacketFactory packetFactory,
            ITransportHandler transportHandler)
        {
            ServerSessionContext = serverSessionContext;
            _packetFactory = packetFactory;
            TransportHandler = transportHandler;
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
            if (resultExecution.Length <= PacketConstants.PacketPayloadThresholdSize)
            {
                var resultExecutionLength = resultExecution.Length;

                Array.Resize(ref resultExecution, PacketConstants.PacketPayloadThresholdSize);

                var answerPacket = _packetFactory
                    .CreateAnswerExecuteSuccessPayload(
                        clientPacket.PacketId, 
                        resultExecution, 
                        resultExecutionLength);

                TransportHandler.Send(answerPacket);
            }
            else
            {
                UpdateTransmitBuffer(resultExecution);

                var answerPacket = _packetFactory
                    .CreateAnswerExecuteSuccessBuffer(
                        clientPacket.PacketId, 
                        ServerSessionContext.TransmitBuffer.Length);

                TransportHandler.Send(answerPacket);
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
