﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.NetworkLayer.TransportHandler;
using NetworkSocketServer.TransportLayer.DTO;
using NetworkSocketServer.TransportLayer.PacketFactory;
using NetworkSocketServer.TransportLayer.Serializer;
using NetworkSocketServer.TransportLayer.ServiceHandlers;

namespace NetworkSocketServer.TransportLayer.PacketHandler
{
    class ServerPredictBasedPacketHandler : IPacketHandler
    {
        private readonly IRequestHandlerFactory _requestHandlerFactory;
        private readonly ITransportHandler _transportHandler;
        private readonly SessionContext _sessionContext;
        private readonly IPacketFactory _packetFactory;
        private readonly IByteSerializer _byteSerializer;

        private readonly IDictionary<PacketClientCommand, Func<Packet, Task>> _handlers;

        public ServerPredictBasedPacketHandler(
            Guid sessionId,
            IRequestHandlerFactory requestHandlerFactory,
            ITransportHandler transportHandler,
            SessionContext sessionContext)
        {
            _packetFactory = new PacketFactory.PacketFactory(sessionId);
            _byteSerializer = new BinaryFormatterByteSerializer();

            _requestHandlerFactory = requestHandlerFactory;
            _transportHandler = transportHandler;
            _sessionContext = sessionContext;

            _handlers = new Dictionary<PacketClientCommand, Func<Packet, Task>>
            {
                { PacketClientCommand.Read, HandleReadCommand },
                { PacketClientCommand.Write, HandleWriteCommand },
                { PacketClientCommand.ExecuteBuffer, HandleExecuteBufferCommand },
                { PacketClientCommand.ExecutePayload, HandleExecutePayloadCommand },
            };
        }


        private Task HandleWriteCommand(Packet packet)
        {
            if (_sessionContext.ReceiveBuffer.Length == 0)
            {
                _sessionContext.ReceiveBuffer.Clear();
                _sessionContext.ReceiveBuffer.SetLength(packet.Position);
            }

            _sessionContext.ReceiveBuffer.Append(packet.Payload);

            var answerPacket = _packetFactory.CreateAnswerSuccessWrite(
                _sessionContext.ReceiveBuffer.Length,
                packet.Payload.Length);

            _transportHandler.Send(_byteSerializer.Serialize(answerPacket));

            return Task.CompletedTask;
        }

        private Task HandleReadCommand(Packet packet)
        {
            if (_sessionContext.TransmitBuffer.Length == 0)
            {
                SendErrorPacket("Transmit Buffer is empty");

                return Task.CompletedTask;
            }

            var array =_sessionContext.TransmitBuffer.Get(packet.Offset, packet.Position);

            var answerPacket = _packetFactory.CreateAnswerSuccessRead(
                    array, 
                    _sessionContext.TransmitBuffer.Length, 
                    array.Length);

            _transportHandler.Send(_byteSerializer.Serialize(answerPacket));

            return Task.CompletedTask;
        }

        private Task HandleExecuteBufferCommand(Packet packet)
        {
            _sessionContext.TransmitBuffer.Clear();
            _sessionContext.TransmitBuffer.SetLength(0);

            throw new NotImplementedException();
        }

        private async Task HandleExecutePayloadCommand(Packet packet)
        {
            _sessionContext.ReceiveBuffer.Clear();
            _sessionContext.ReceiveBuffer.SetLength(0);
            _sessionContext.TransmitBuffer.Clear();
            _sessionContext.TransmitBuffer.SetLength(0);

            var request = _byteSerializer.Deserialize<Request>(packet.Payload);

            var response = await _requestHandlerFactory.CreateRequestHandler().HandleRequest(request);

            var responseBytes = _byteSerializer.Serialize(response);

            if (responseBytes.Length * 2 <= _sessionContext.PacketPayloadThreshold)
            {
                var answer = _packetFactory.CreateAnswerExecuteSuccessPayload(responseBytes, responseBytes.Length);

                _transportHandler.Send(_byteSerializer.Serialize(answer));
            }
            else
            {
                _sessionContext.TransmitBuffer.Append(responseBytes);

                var answer = _packetFactory.CreateAnswerExecuteSuccessBuffer(_sessionContext.TransmitBuffer.Length);

                _transportHandler.Send(_byteSerializer.Serialize(answer));
            }
        }

        public async Task<bool> HandlePacket(Packet packet)
        {
            try
            {
                if (packet.PacketClientCommand == PacketClientCommand.Close)
                {
                    _transportHandler.Close();

                    return false;
                }   
                if (_handlers.TryGetValue(packet.PacketClientCommand, out var handler))
                {
                    await handler(packet);

                    return true;
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported command:" + nameof(packet.PacketClientCommand));
                }
            }
            catch (Exception exception)
            {
                SendErrorPacket(exception.Message);

                return true;
            }
        }

        private void SendErrorPacket(string text)
        {
            var errorAnswerPacket = _packetFactory.CreateAnswerError(text);

            _transportHandler.Send(_byteSerializer.Serialize(errorAnswerPacket));
        }
    }
}