﻿using System;
using System.Linq;
using NetworkSimpleServer.NetworkLayer.Client.ClientTransportHandler;
using NetworkSimpleServer.NetworkLayer.Core;
using NetworkSimpleServer.NetworkLayer.Core.Logger;
using NetworkSimpleServer.NetworkLayer.Core.Packets;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.TransportLayer.Client.TransportManager.Handlers;
using NetworkSocketServer.TransportLayer.Core.Packets.Factory;
using NetworkSocketServer.TransportLayer.Core.Serializer;

namespace NetworkSocketServer.TransportLayer.Client.TransportManager
{
    class ClientTransportManager : IClientTransportManager
    {
        private readonly ILogger _logger;
        private readonly IPacketFactory _packetFactory;
        private readonly IClientTransportHandler _clientTransportHandler;
        private readonly IBytesSerializer _bytesSerializer;

        public ClientTransportManager(
            ILogger logger,
            IPacketFactory packetFactory,
            IClientTransportHandler clientTransportHandler, 
            IBytesSerializer bytesSerializer)
        {
            _logger = logger;
            _packetFactory = packetFactory;
            _clientTransportHandler = clientTransportHandler;
            _bytesSerializer = bytesSerializer;
        }

        public Response SendRequest(Request request)
        {
            var requestBytes = _bytesSerializer.Serialize(request);

            var executePacket = ProcessExecutePacket(requestBytes);

            var answerPacket = _clientTransportHandler.AcceptedSend(executePacket);

            var responseBytes =  HandleExecuteAnswer(answerPacket);

            return _bytesSerializer.Deserialize<Response>(responseBytes);
        }

        private Packet ProcessExecutePacket(byte[] requestBytes)
        {
            if (requestBytes.Length <= PacketConstants.PacketPayloadThresholdSize)
            {
                var requestLength = requestBytes.Length;

                Array.Resize(ref requestBytes, PacketConstants.PacketPayloadThresholdSize);

                return _packetFactory.CreateExecutedInPayload(requestBytes, requestLength);
            }
            else
            {
                var handler = new BufferedRequestHandler(
                    PacketConstants.PacketPayloadThresholdSize,
                    _packetFactory,
                    _clientTransportHandler,
                    _logger);

                return handler.ProvideRequestToServerBuffer(requestBytes);
            }
        }
        
        private byte[] HandleExecuteAnswer(Packet answerPacket)
        {
            if (answerPacket.PacketServerResponse == PacketServerResponse.ResultInPayload)
            {
                var payload = answerPacket.Payload.Take(answerPacket.PayloadSize).ToArray();

                return payload;
            }
            else
            {
                var handler = new BufferedResponseHandler(
                    PacketConstants.PacketPayloadThresholdSize,
                    _packetFactory,
                    _logger,
                    _clientTransportHandler);

                return handler.GetResponseFromServerBuffer(answerPacket.BuffferSize, 1);
            }
        }
    }
}
