using NetworkSocketServer.TransportLayer.DTO;

namespace NetworkSocketServer.TransportLayer.PacketFactory
{
    interface IPacketFactory
    {
        Packet CreateAnswerSuccessWrite(int bufferSize, int bytesWrite);

        Packet CreateAnswerError(string text);

        Packet CreateAnswerSuccessRead(byte[] array, int transmitBufferLength, int arrayLength);

        Packet CreateAnswerExecuteSuccessPayload(byte[] responseBytes, int responseBytesLength);

        Packet CreateAnswerExecuteSuccessBuffer(int transmitBufferLength);

        Packet CreateClosePacket();

        Packet CreateExecuteBuffer(int requestByteLength);

        Packet CreateExecutePayload(byte[] requestBytes);

        Packet CreateWrite(byte[] data, int position, int offset);

        Packet CreateRead(int position, int offset);
    }
}
