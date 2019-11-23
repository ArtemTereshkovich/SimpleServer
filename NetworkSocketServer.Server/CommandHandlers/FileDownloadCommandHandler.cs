using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetworkSocketServer.Commands;
using NetworkSocketServer.Messages;
using NetworkSocketServer.Network.TransportHandler;

namespace NetworkSocketServer.Server.CommandHandlers
{
    internal class FileDownloadCommandHandler : ICommandHandler
    {
        private readonly ITransportHandler _transportHandler;

        public FileDownloadCommandHandler(ITransportHandler transportHandler)
        {
            _transportHandler = transportHandler;
        }


        public async Task Handle(Command command)
        {
            var fileCommand = command as FileInfoCommand; 

            var serverRootFileName = $"Resources{Path.DirectorySeparatorChar}{fileCommand.FileName}";

            if (!Directory.Exists($"Resources{Path.DirectorySeparatorChar}{fileCommand.ClientId}"))
            {
                Directory.CreateDirectory($"Resources{Path.DirectorySeparatorChar}{command.ClientId}");
            }
            var userFolderFileName = $"Resources{Path.DirectorySeparatorChar}{fileCommand.ClientId}{Path.DirectorySeparatorChar}{fileCommand.FileName}";
            var localFileName = File.Exists(userFolderFileName) ? userFolderFileName : serverRootFileName;

            if (!File.Exists(localFileName))
            {
                var fileNotFoundResponse = new FileInfoCommand()
                {
                    CommandType = CommandType.DownloadFileResponse
                };
                await _transportHandler.Send(fileNotFoundResponse.Serialize());

                return;
            }

            var fileInfo = new FileInfo(localFileName);
            var response = new FileInfoCommand()
            {
                CommandType = CommandType.DownloadFileResponse,
                FileName = fileCommand.FileName,
                IsExist = fileInfo.Exists,
                Size = fileInfo.Length
            };

            await _transportHandler.Send(response.Serialize());

            (await _transportHandler.Receive()).Deserialize<FileInfoCommand>();

            var bytes = File.ReadAllBytes(localFileName).Skip((int)fileCommand.Size).ToArray();

            await _transportHandler.Send(bytes);
        }
    }
}