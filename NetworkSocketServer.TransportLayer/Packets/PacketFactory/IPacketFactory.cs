using System.ComponentModel;

namespace NetworkSocketServer.TransportLayer.Packets.PacketFactory
{
    interface IPacketFactory
    {
        Packet CreateAnswerSuccessWrite(
            int receiveBufferSize,
            int receiveBufferOffset,
            int payloadSize);

        Packet CreateAnswerSuccessRead(
            byte[] payload,
            int transmitBufferSize,
            int transmitBufferOffset,
            int payloadSize);

        Packet CreateAnswerExecuteSuccessPayload(
            byte[] payload,
            int payloadSize);

        Packet CreateAnswerExecuteSuccessBuffer(int transmitBufferLength);

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
