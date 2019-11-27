using System;
using System.Text;
using NetworkSocketServer.TransportLayer.DTO;

namespace NetworkSocketServer.TransportLayer.PacketFactory
{
    class PacketFactory : IPacketFactory
    {
        private readonly Guid _sessionId;

        public PacketFactory(Guid sessionId)
        {
            _sessionId = sessionId;
        }

        public Packet CreateAnswerSuccessWrite(int bufferSize, int bytesWrite)
        {
            return new Packet
            {
                SessionId = _sessionId,
                Size = bufferSize,
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
                SessionId = _sessionId,
                Size = 0,
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
                SessionId = _sessionId,
                PacketClientCommand = PacketClientCommand.None,
                PacketServerResponse = PacketServerResponse.Answer,
                Payload = array,
                Offset = arrayLength,
                Size = transmitBufferLength
            };
        }

        public Packet CreateAnswerExecuteSuccessPayload(byte[] responseBytes, int responseBytesLength)
        {
            return new Packet
            {
                Offset = responseBytesLength,
                PacketServerResponse = PacketServerResponse.ResultInPayload,
                PacketClientCommand = PacketClientCommand.None,
                Payload = responseBytes,
                Size = 0,
                SessionId = _sessionId
            };
        }

        public Packet CreateAnswerExecuteSuccessBuffer(int transmitBufferLength)
        {
            return new Packet
            {
                Size = 0,
                Offset = transmitBufferLength,
                PacketClientCommand = PacketClientCommand.None,
                PacketServerResponse = PacketServerResponse.ResultInBuffer,
                SessionId = _sessionId,
                Payload = null,
            };
        }

        public Packet CreateClosePacket()
        {
            return new Packet
            {
                Offset = 0,
                Size = 0,
                PacketServerResponse = PacketServerResponse.Answer,
                PacketClientCommand = PacketClientCommand.Close,
                Payload = null,
                SessionId = _sessionId
            };
        }

        public Packet CreateExecuteBuffer(int requestByteLength)
        {
            return new Packet
            {
                Size = 0,
                Offset = requestByteLength,
                SessionId = _sessionId,
                PacketServerResponse = PacketServerResponse.Answer,
                PacketClientCommand = PacketClientCommand.ExecuteBuffer,
                Payload = null
            };
        }

        public Packet CreateExecutePayload(byte[] requestBytes)
        {
            return new Packet
            {
                Offset = requestBytes.Length,
                Size = 0,
                PacketServerResponse = PacketServerResponse.Answer,
                PacketClientCommand = PacketClientCommand.ExecutePayload,
                Payload = requestBytes,
                SessionId = _sessionId
            };
        }

        public Packet CreateWrite(byte[] data, int size, int offset)
        {
            return new Packet
            {
                SessionId = _sessionId,
                Offset = offset,
                Size = size,
                PacketClientCommand = PacketClientCommand.Write,
                PacketServerResponse = PacketServerResponse.Answer,
                Payload = data
            };
        }

        public Packet CreateRead(int position, int offset)
        {
            return new Packet
            {
                SessionId = _sessionId,
                Offset = offset,
                Size = position,
                PacketClientCommand = PacketClientCommand.Read,
                PacketServerResponse = PacketServerResponse.Answer,
                Payload = null,
            };
        }
    }
}
