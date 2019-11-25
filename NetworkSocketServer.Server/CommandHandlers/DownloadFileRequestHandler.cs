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
        public Task<Response> Handle(Request request)
        {
            var downloadFileRequest = request as DownloadFileRequest; 

            /*var serverRootFileName = $"Resources{Path.DirectorySeparatorChar}{fileCommand.FileName}";

            if (!Directory.Exists($"Resources{Path.DirectorySeparatorChar}{fileCommand.ConnectionId}"))
            {
                Directory.CreateDirectory($"Resources{Path.DirectorySeparatorChar}{request.ConnectionId}");
            }
            var userFolderFileName = $"Resources{Path.DirectorySeparatorChar}{fileCommand.ConnectionId}{Path.DirectorySeparatorChar}{fileCommand.FileName}";
            var localFileName = File.Exists(userFolderFileName) ? userFolderFileName : serverRootFileName;

            if (!File.Exists(localFileName))
            {
                var fileNotFoundResponse = new UploadFileRequest()
                {
                    CommandType = CommandType.DownloadFileResponse
                };
                await _transportHandler.Send(fileNotFoundResponse.Serialize());

                return;
            }

            var fileInfo = new FileInfo(localFileName);
            var response = new UploadFileRequest()
            {
                CommandType = CommandType.DownloadFileResponse,
                FileName = fileCommand.FileName,
                IsExist = fileInfo.Exists,
                Size = fileInfo.Length
            };

            await _transportHandler.Send(response.Serialize());

            (await _transportHandler.Receive()).Deserialize<UploadFileRequest>();

            var bytes = File.ReadAllBytes(localFileName).Skip((int)fileCommand.Size).ToArray();

            await _transportHandler.Send(bytes);*/

            var response = new DownloadFileResponse
            {
                ConnectionId = request.ConnectionId,
                File = null,
                FileSize = 0,
                Filename = downloadFileRequest.Filename
            };

            return Task.FromResult((Response)response);
        }
    }
}