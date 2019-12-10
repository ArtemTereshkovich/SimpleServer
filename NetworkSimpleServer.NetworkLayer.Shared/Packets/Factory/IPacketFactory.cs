using System;

namespace NetworkSimpleServer.NetworkLayer.Core.Packets.Factory
{
    interface IPacketFactory
    {
        Packet CreateAnswerSuccessWrite(
            Guid packetId,
            int receiveBufferSize,
            int receiveBufferOffset,
            int payloadSize);

        Packet CreateAnswerSuccessRead(
            Guid packetId,
            byte[] payload,
            int transmitBufferSize,
            int transmitBufferOffset,
            int payloadSize);

        Packet CreateAnswerExecuteSuccessPayload(
            Guid packetId,
            byte[] payload,
            int payloadSize);

        Packet CreateAnswerExecuteSuccessBuffer(Guid packetId, int transmitBufferLength);

        Packet CreateClosePacket();

        Packet CreateExecutedInBuffer(int transmitBufferSize);

        Packet CreateExecutedInPayload(byte[] payload, int payloadSize);

        Packet CreateWrite(
            byte[] payload,
            int receiveBufferSize,
            int receiveBufferOffset,
            int payloadSize);

        Packet CreateRead(
            int transmitBufferSize,
            int transmitBufferOffset);
    }
}
