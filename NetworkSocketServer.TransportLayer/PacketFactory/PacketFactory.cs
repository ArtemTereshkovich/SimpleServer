using System;
using System.Text;
using NetworkSocketServer.TransportLayer.DTO;

namespace NetworkSocketServer.TransportLayer.PacketFactory
{
    class PacketFactory : IPacketFactory
    {
        private readonly Guid _connectionId;

        public PacketFactory(Guid connectionId)
        {
            _connectionId = connectionId;
        }

        public Packet CreateAnswerSuccessWrite(int bufferSize, int bytesWrite)
        {
            return new Packet
            {
                SessionId = _connectionId,
                Position = bufferSize,
                Offset = bytesWrite,
                PacketClientCommand = PacketClientCommand.None,
                PacketServerResponse = PacketServerResponse.Answer,
                Payload = null,
            };
        }

        public Packet CreateAnswerError(string text)
        {
            return new Packet
            {
                SessionId = _connectionId,
                Position = 0,
                Offset = 0,
                PacketClientCommand = PacketClientCommand.None,
                PacketServerResponse = PacketServerResponse.Error,
                Payload = Encoding.ASCII.GetBytes(text),
            };
        }

        public Packet CreateAnswerSuccessRead(byte[] array, int transmitBufferLength, int arrayLength)
        {
            return new Packet
            {

            };
        }

        public Packet CreateAnswerExecuteSuccessPayload(byte[] responseBytes, int responseBytesLength)
        {
            throw new NotImplementedException();
        }

        public Packet CreateAnswerExecuteSuccessBuffer(int transmitBufferLength)
        {
            throw new NotImplementedException();
        }
    }
}
