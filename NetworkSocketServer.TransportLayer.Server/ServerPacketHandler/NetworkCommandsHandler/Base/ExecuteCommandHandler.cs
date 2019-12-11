using System;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Core;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSimpleServer.NetworkLayer.Core.TransportHandler;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;

namespace NetworkSocketServer.TransportLayer.Server.ServerPacketHandler.NetworkCommandsHandler.Base
{
    abstract class ExecuteCommandHandler : INetworkCommandHandler 
    {
        protected readonly ServerSessionContext ServerSessionContext;
        private readonly IPacketFactory _packetFactory;
        private readonly ITransportHandler _transportHandler;

        protected ExecuteCommandHandler(
            ServerSessionContext serverSessionContext,
            IPacketFactory packetFactory,
            ITransportHandler transportHandler)
        {
            ServerSessionContext = serverSessionContext;
            _packetFactory = packetFactory;
            _transportHandler = transportHandler;
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

                _transportHandler.Send(answerPacket);
            }
            else
            {
                UpdateTransmitBuffer(resultExecution);

                var answerPacket = _packetFactory
                    .CreateAnswerExecuteSuccessBuffer(
                        clientPacket.PacketId, 
                        ServerSessionContext.TransmitBuffer.Length);

                _transportHandler.Send(answerPacket);
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
