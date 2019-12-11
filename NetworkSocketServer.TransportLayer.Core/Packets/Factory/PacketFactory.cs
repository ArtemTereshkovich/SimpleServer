using System;
using NetworkSimpleServer.NetworkLayer.Core;
using NetworkSimpleServer.NetworkLayer.Core.Packets;

namespace NetworkSocketServer.TransportLayer.Core.Packets.Factory
{
    public class PacketFactory : IPacketFactory
    {
        private readonly Guid _sessionId;

        public PacketFactory(Guid sessionId)
        {
            _sessionId = sessionId;
        }  

        public Packet CreateAnswerSuccessWrite(
            Guid packetId,
            int receiveBufferSize,
            int receiveBufferOffset,
            int payloadSize)
        {
            return new Packet
            {
                PacketId =  packetId,
                SessionId = _sessionId,
                BuffferSize = receiveBufferSize,
                BufferOffset = receiveBufferOffset,
                PacketClientCommand = PacketClientCommand.None,
                PacketServerResponse = PacketServerResponse.Answer,
                PayloadSize = payloadSize,
                Payload = PacketConstants.EmptyPayload,
            };
        }
        
        public Packet CreateAnswerSuccessRead(
            Guid packetId,
            byte[] payload, 
            int transmitBufferSize, 
            int transmitBufferOffset, 
            int payloadSize)
        {
            return new Packet
            {
                PacketId = packetId,
                SessionId = _sessionId,
                PacketClientCommand = PacketClientCommand.None,
                PacketServerResponse = PacketServerResponse.Answer,
                Payload = payload,
                BufferOffset = transmitBufferOffset,
                BuffferSize = transmitBufferSize,
                PayloadSize = payloadSize
            };
        }

        public Packet CreateAnswerExecuteSuccessPayload(
            Guid packetId,
            byte[] payload,
            int payloadSize)
        {
            return new Packet
            {
                PacketId = packetId,
                BufferOffset = 0,
                PacketServerResponse = PacketServerResponse.ResultInPayload,
                PacketClientCommand = PacketClientCommand.None,
                Payload = payload,
                PayloadSize = payloadSize,
                BuffferSize = 0,
                SessionId = _sessionId
            };
        }

        public Packet CreateAnswerExecuteSuccessBuffer(
            Guid packetId, int transmitBufferLength)
        {
            return new Packet
            {
                PacketId = packetId,
                BuffferSize = transmitBufferLength,
                BufferOffset = 0,
                PayloadSize = 0,
                PacketClientCommand = PacketClientCommand.None,
                PacketServerResponse = PacketServerResponse.ResultInBuffer,
                SessionId = _sessionId,
                Payload = PacketConstants.EmptyPayload,
            };
        }

        public Packet CreateClosePacket()
        {
            return new Packet
            {
                BufferOffset = 0,
                BuffferSize = 0,
                PacketServerResponse = PacketServerResponse.Answer,
                PacketClientCommand = PacketClientCommand.Close,
                Payload = PacketConstants.EmptyPayload,
                PayloadSize = 0,
                SessionId = _sessionId,
                PacketId = Guid.NewGuid()
            };
        }

        public Packet CreateExecutedInBuffer(int transmitBufferSize)
        {
            return new Packet
            {
                BuffferSize = transmitBufferSize,
                BufferOffset = 0,
                SessionId = _sessionId,
                PacketServerResponse = PacketServerResponse.Answer,
                PacketClientCommand = PacketClientCommand.ExecuteBuffer,
                Payload = PacketConstants.EmptyPayload,
                PacketId = Guid.NewGuid()
            };
        }

        public Packet CreateExecutedInPayload(byte[] payload, int payloadSize)
        {
            return new Packet
            {
                BufferOffset = 0,
                BuffferSize = 0,
                PacketServerResponse = PacketServerResponse.Answer,
                PacketClientCommand = PacketClientCommand.ExecutePayload,
                Payload = payload,
                PayloadSize = payloadSize,
                SessionId = _sessionId,
                PacketId = Guid.NewGuid()
            };
        }

        public Packet CreateWrite(
            byte[] payload,
            int receiveBufferSize,
            int receiveBufferOffset,
            int payloadSize)
        {
            return new Packet
            {
                SessionId = _sessionId,
                BufferOffset = receiveBufferOffset,
                BuffferSize = receiveBufferSize,
                PacketClientCommand = PacketClientCommand.Write,
                PacketServerResponse = PacketServerResponse.Answer,
                Payload = payload,
                PayloadSize = payloadSize,
                PacketId = Guid.NewGuid()
            };
        }

        public Packet CreateRead(
            int transmitBufferSize, 
            int transmitBufferOffset)
        {
            return new Packet
            {
                SessionId = _sessionId,
                BufferOffset = transmitBufferOffset,
                BuffferSize = transmitBufferSize,
                PacketClientCommand = PacketClientCommand.Read,
                PacketServerResponse = PacketServerResponse.Answer,
                Payload = PacketConstants.EmptyPayload,
                PayloadSize = 0,
                PacketId = Guid.NewGuid()
            };
        }
    }
}
