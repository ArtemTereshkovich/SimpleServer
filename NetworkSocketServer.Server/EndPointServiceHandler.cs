using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetworkSocketServer.Messages;
using NetworkSocketServer.Network;
using NetworkSocketServer.Network.TransportHandler;

namespace NetworkSocketServer.Server
{
    class EndPointServiceHandler : INetworkServiceHandler
    {
        public async Task HandleNewConnection(ITransportHandler transportHandler)
        {
            await (new SimpleServiceHandler(transportHandler)).StartReceive();
        }
    }

    internal class SimpleServiceHandler
    {
        private readonly ITransportHandler _transportHandler;

        public SimpleServiceHandler(ITransportHandler transportHandler)
        {
            _transportHandler = transportHandler;
        }

        public async Task StartReceive()
        {
            var requstBytes = await _transportHandler.Receive();

            if (requstBytes.Length == 0)
            {
                Console.WriteLine($"Disconnected from server!");

                _transportHandler.Close();
            }
            else
            {
                
                var request = requstBytes.Deserialize<Message>();

                Console.WriteLine($"Message received: {request.MessageType}");

                switch (request.MessageType)
                {
                    case MessageType.UploadFileRequest:
                        await UploadFile(request as FileInfoMessage);
                        break;
                    case MessageType.DownloadFileRequest:
                        await DownloadFile(request as FileInfoMessage);
                        break;
                    case MessageType.EchoRequest:
                        await Echo(request as TextMessage);
                        break;
                    case MessageType.TimeRequest:
                        await Time(request);
                        break;
                }
            }
        }

        public async Task UploadFile(FileInfoMessage message)
        {
            if (!Directory.Exists($"Resources{Path.DirectorySeparatorChar}{message.ClientId}"))
            {
                Directory.CreateDirectory($"Resources{Path.DirectorySeparatorChar}{message.ClientId}");
            }
            var localFileName = $"Resources{Path.DirectorySeparatorChar}{message.ClientId}{Path.DirectorySeparatorChar}{message.FileName}";

            var fileInfo = new FileInfo(localFileName);
            var fileLength = fileInfo.Exists ? fileInfo.Length : 0;

            var serverFileInfoResponse = new FileInfoMessage()
            {
                MessageType = MessageType.UploadFileResponse,
                FileName = message.FileName,
                IsExist = fileInfo.Exists,
                Size = fileLength
            };

            await _transportHandler.Send(serverFileInfoResponse.Serialize());

            await using (var fileStream = File.OpenWrite(localFileName))
            await using (var binaryWriter = new BinaryWriter(fileStream))
            {
                binaryWriter.BaseStream.Seek(0, SeekOrigin.End);

                var bytesReceived = 0;
                while (bytesReceived < message.Size - fileLength)
                {
                    var filePart = await _transportHandler.Receive();

                    binaryWriter.Write(filePart);
                    binaryWriter.Flush();

                    bytesReceived += filePart.Length;
                }
            }

            var response = new Message()
            {
                MessageType = MessageType.UploadFileResponse
            };

            await _transportHandler.Send(response.Serialize());
        }

        public async Task DownloadFile(FileInfoMessage message)
        {
            var serverRootFileName = $"Resources{Path.DirectorySeparatorChar}{message.FileName}";

            if (!Directory.Exists($"Resources{Path.DirectorySeparatorChar}{message.ClientId}"))
            {
                Directory.CreateDirectory($"Resources{Path.DirectorySeparatorChar}{message.ClientId}");
            }
            var userFolderFileName = $"Resources{Path.DirectorySeparatorChar}{message.ClientId}{Path.DirectorySeparatorChar}{message.FileName}";
            var localFileName = File.Exists(userFolderFileName) ? userFolderFileName : serverRootFileName;

            if (!File.Exists(localFileName))
            {
                var fileNotFoundResponse = new FileInfoMessage()
                {
                    MessageType = MessageType.DownloadFileResponse
                };
                await _transportHandler.Send(fileNotFoundResponse.Serialize());

                return;
            }

            var fileInfo = new FileInfo(localFileName);
            var response = new FileInfoMessage()
            {
                MessageType = MessageType.DownloadFileResponse,
                FileName = message.FileName,
                IsExist = fileInfo.Exists,
                Size = fileInfo.Length
            };

            await _transportHandler.Send(response.Serialize());

            (await _transportHandler.Receive()).Deserialize<FileInfoMessage>();

            var bytes = File.ReadAllBytes(localFileName).Skip((int)message.Size).ToArray();

            await _transportHandler.Send(bytes);
        }

        public async Task Echo(TextMessage message)
        {
            var response = new TextMessage
            {
                MessageType = MessageType.TextResponse,
                Text = message.Text
            };

            await _transportHandler.Send(response.Serialize());
        }

        public async Task Time(Message _)
        {
            var response = new TextMessage
            {
                MessageType = MessageType.TextResponse,
                Text = $"{DateTime.Now:dddd, dd MMMM yyyy HH:mm:ss}"
            };

            await _transportHandler.Send(response.Serialize());
        }
    }
}
