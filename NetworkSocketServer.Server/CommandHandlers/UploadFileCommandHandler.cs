using System.IO;
using System.Threading.Tasks;
using NetworkSocketServer.Commands;
using NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.Server.CommandHandlers
{
    internal class UploadFileCommandHandler : ICommandHandler
    {
        private readonly ITransportHandler _transportHandler;

        public UploadFileCommandHandler(ITransportHandler transportHandler)
        {
            _transportHandler = transportHandler;
        }

        public async Task Handle(Command command)
        {
            var fileCommand = command as FileInfoCommand;

            if (!Directory.Exists($"Resources{Path.DirectorySeparatorChar}{fileCommand.ClientId}"))
            {
                Directory.CreateDirectory($"Resources{Path.DirectorySeparatorChar}{fileCommand.ClientId}");
            }
            var localFileName = $"Resources{Path.DirectorySeparatorChar}{fileCommand.ClientId}{Path.DirectorySeparatorChar}{fileCommand.FileName}";

            var fileInfo = new FileInfo(localFileName);
            var fileLength = fileInfo.Exists ? fileInfo.Length : 0;

            var serverFileInfoResponse = new FileInfoCommand()
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

            var response = new Command()
            {
                CommandType = CommandType.UploadFileResponse
            };

            await _transportHandler.Send(response.Serialize());
        }
    }
}