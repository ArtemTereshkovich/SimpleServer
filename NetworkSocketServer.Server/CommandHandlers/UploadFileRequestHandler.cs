using System;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;

namespace NetworkSocketServer.Server.CommandHandlers
{
    internal class UploadFileRequestHandler : ICommandHandler
    {
        public Task<Response> Handle(Request request)
        {
            var uploadFileRequest = request as UploadFileRequest;
            /*
            var fileCommand = command as UploadFileRequest;

            if (!Directory.Exists($"Resources{Path.DirectorySeparatorChar}{fileCommand.ConnectionId}"))
            {
                Directory.CreateDirectory($"Resources{Path.DirectorySeparatorChar}{fileCommand.ConnectionId}");
            }
            var localFileName = $"Resources{Path.DirectorySeparatorChar}{fileCommand.ConnectionId}{Path.DirectorySeparatorChar}{fileCommand.FileName}";

            var fileInfo = new FileInfo(localFileName);
            var fileLength = fileInfo.Exists ? fileInfo.Length : 0;

            var serverFileInfoResponse = new UploadFileRequest()
            {
                CommandType = CommandType.UploadFileResponse,
                FileName = fileCommand.FileName,
                IsExist = fileInfo.Exists,
                Size = fileLength
            };

            await _transportHandler.Send(serverFileInfoResponse.Serialize());

            await using (var fileStream = File.OpenWrite(localFileName))
            await using (var binaryWriter = new BinaryWriter(fileStream))
            {
                binaryWriter.BaseStream.Seek(0, SeekOrigin.End);

                var bytesReceived = 0;
                while (bytesReceived < fileCommand.Size - fileLength)
                {
                    var filePart = await _transportHandler.Receive();

                    binaryWriter.Write(filePart);
                    binaryWriter.Flush();

                    bytesReceived += filePart.Length;
                }
            }

            var response = new Request()
            {
                CommandType = CommandType.UploadFileResponse
            };

            await _transportHandler.Send(response.Serialize());*/

            var response = new UploadFileResponse
            {
                ConnectionId = uploadFileRequest.ConnectionId,
                Filename = uploadFileRequest.FileName,
            };

            return Task.FromResult((Response) response);
        }
    }
}