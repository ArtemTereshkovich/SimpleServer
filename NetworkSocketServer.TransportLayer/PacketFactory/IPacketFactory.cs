using NetworkSocketServer.TransportLayer.DTO;

namespace NetworkSocketServer.TransportLayer.PacketFactory
{
    interface IPacketFactory
    {
        Packet CreateAnswerSuccessWrite(int bufferSize, int bytesWrite);

        Packet CreateAnswerError(string text);

        Packet CreateAnswerSuccessRead(byte[] array, int transmitBufferLength, int arrayLength);
    }
}
