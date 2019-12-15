using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.TransportLayer.Server.IRequestHandler;

namespace NetworkSocketServer.Server.ServiceHandlers
{
    class DownloadRequestHandler : IRequestHandler
    {
        public async Task<Response> HandleRequest(Request request)
        {
            if (request is DownloadFileRequest downloadFileRequest)
            {
                return await HandleDownloadFileRequest(downloadFileRequest);
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

        private async Task<DownloadFileResponse> HandleDownloadFileRequest(DownloadFileRequest downloadFileRequest)
        {
            var localFileName = $"Files{Path.DirectorySeparatorChar}{downloadFileRequest.Filename}";

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
