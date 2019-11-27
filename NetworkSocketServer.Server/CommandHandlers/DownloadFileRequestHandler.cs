using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;

namespace NetworkSocketServer.Server.CommandHandlers
{
    internal class DownloadFileRequestHandler : ICommandHandler
    {
        public async Task<Response> Handle(Request request)
        {
            var downloadFileRequest = request as DownloadFileRequest;

            var localFileName = $"Files{Path.DirectorySeparatorChar}{downloadFileRequest.Filename}";

            var fileInfo = new FileInfo(localFileName);

            await Task.Delay(1);

            var bytes = File.ReadAllBytes(localFileName).ToArray();

            return new DownloadFileResponse
            {
                ErrorMessage = "",
                Filename = downloadFileRequest.Filename,
                File = bytes,
                FileSize = bytes.Length,
                IsSuccess = true,
                ResponseId = downloadFileRequest.RequestId
            };
        }
    }
}