using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;

namespace NetworkSocketServer.Server.CommandHandlers
{
    internal class UploadFileRequestHandler : ICommandHandler
    {
        public async Task<Response> Handle(Request request)
        {
            
            var fileCommand = request as UploadFileRequest;

            var localFileName = $"Files{Path.DirectorySeparatorChar}{fileCommand.FileName}";

            var fileInfo = new FileInfo(localFileName);

            await using (var fileStream = File.OpenWrite(localFileName))
            await using (var binaryWriter = new BinaryWriter(fileStream))
            {
                binaryWriter.Write(fileCommand.File);
                binaryWriter.Flush();
            }


            Console.Write("Receive File Upload Request");

            return new UploadFileResponse
            {
                ResponseId = fileCommand.RequestId,
                Filename = fileCommand.FileName,
            };
        }
    }
}