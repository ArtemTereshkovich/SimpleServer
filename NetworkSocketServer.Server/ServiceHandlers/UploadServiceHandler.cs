using System;
using System.IO;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;

namespace NetworkSocketServer.Server.ServiceHandlers
{
    class UploadServiceHandler : IRequestHandler
    {
        public async Task<Response> HandleRequest(Request request)
        {
            if (request is UploadFileRequest uploadFileRequest)
            {
                return await HandleUploadFileRequest(uploadFileRequest);
            }
            else
            {
                return new ErrorResponse
                {
                    ErrorText = "Unsupported Command.",
                    ResponseId = request.RequestId,
                };
            }
        }

        private async Task<UploadFileResponse> HandleUploadFileRequest(UploadFileRequest uploadFileRequest)
        {
            var localFileName = $"Files{Path.DirectorySeparatorChar}{uploadFileRequest.FileName}";
            
            await using (var fileStream = File.OpenWrite(localFileName))
            await using (var binaryWriter = new BinaryWriter(fileStream))
            {
                binaryWriter.Write(uploadFileRequest.File);
                binaryWriter.Flush();
            }
            
            Console.Write("Receive File Upload Request");

            return new UploadFileResponse
            {
                ResponseId = uploadFileRequest.RequestId,
                Filename = uploadFileRequest.FileName,
            };
        }
    }
}
