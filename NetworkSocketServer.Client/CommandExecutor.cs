using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetworkSimpleServer.NetworkLayer.Client.Connectors;
using NetworkSocketServer.Client.Commands;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.TransportLayer.Client.ConnectionManager;

namespace NetworkSocketServer.Client
{
    public class CommandExecutor
    {
        private readonly IClientConnectionManager _clientConnectionManager;

        public CommandExecutor(IClientConnectionManager clientConnectionManager)
        {
            _clientConnectionManager = clientConnectionManager;
        }

        public void Execute(HelpCommand _)
        {
            Console.WriteLine("Supported commands:");
            Console.WriteLine("-connect [ip]:[port]");
            Console.WriteLine("-disconnect");
            Console.WriteLine("-date");
            Console.WriteLine("-text [message]");
            Console.WriteLine("-upload [file]");
            Console.WriteLine("-download [file]");
        }

        public void Execute(ConnectCommand connectTcpCommand)
        {
            if (_clientConnectionManager.IsConnected)
            {
                Console.WriteLine("Already connected!");
            }
            else
            {
                _clientConnectionManager.Connect(new NetworkConnectorSettings
                {
                    IpEndPointServer = connectTcpCommand.EndPoint
                });
            }
        }
        
        public void Execute(TextCommand command)
        {
            if (!_clientConnectionManager.IsConnected)
            {
                Console.WriteLine("Doesnt connected!");
                return;
            }

            var request = new TextRequest()
            {
                RequestId = Guid.NewGuid(),
                Text = command.Message
            };

            var response = _clientConnectionManager.SendRequest(request);

            if (response is ErrorResponse errorResponse)
            {
                HandlerError(errorResponse);
                return;
            }

            var textResponse = response as TextResponse;

            Console.WriteLine($"Execute text command ({textResponse.ResponseId}):{textResponse.Text}");
        }

        public void Execute(DateCommand dateCommand)
        {
            if (!_clientConnectionManager.IsConnected)
            {
                Console.WriteLine("Doesnt connected!");
                return;
            }

            var request = new DateRequest
            {
                RequestId = Guid.NewGuid(),
                ClientDate = dateCommand.ClientDateTime
            };

            var response = _clientConnectionManager.SendRequest(request);

            if (response is ErrorResponse errorResponse)
            {
                HandlerError(errorResponse);
                return;
            }

            var dateResponse = response as DateResponse;

            Console.WriteLine($"Execute date command ({dateResponse.ResponseId}):{dateResponse.ServerTime}. Time offset: {dateResponse.Offset}");
        }
        public void Execute(UploadFileCommand fileCommand)
        {
            if (!_clientConnectionManager.IsConnected)
            {
                Console.WriteLine("Doesnt connected!");
                return ;
            }

            var localFileName = $"Files{Path.DirectorySeparatorChar}{fileCommand.FileName}";
            if (!File.Exists(localFileName))
            {
                Console.WriteLine("File not found!");
                return ;
            }

            var fileInfo = new FileInfo(localFileName);

            var stopwatch = new Stopwatch();
            stopwatch.Restart();

            var bytes = File.ReadAllBytes(localFileName).ToArray();

            var request = new UploadFileRequest
            {
                File = bytes,
                FileName = fileCommand.FileName,
                RequestId = Guid.NewGuid(),
                Size = fileInfo.Length
            };

            var response = _clientConnectionManager.SendRequest(request);

            if (response is ErrorResponse errorResponse)
            {
                HandlerError(errorResponse);
                return;
            }

            var uploadResponse = response as UploadFileResponse;

            stopwatch.Stop();

            Console.WriteLine($"File uploaded successfully! {uploadResponse.ResponseId} " +
                $"Average upload speed is {((double)bytes.Length / (1024 * 1024)) / (((double)stopwatch.ElapsedMilliseconds + 1) / 1000)} Mbps.");
        }

        public async Task Execute(DownloadFileCommand fileCommand)
        {
            if (!_clientConnectionManager.IsConnected)
            {
                Console.WriteLine("Doesnt connected!");
                return;
            }

            var request = new DownloadFileRequest
            {
                RequestId = Guid.NewGuid(),
                Filename = fileCommand.FileName
            };

            var stopwatch = new Stopwatch();
            stopwatch.Restart();

            var response = _clientConnectionManager.SendRequest(request);

            if (response is ErrorResponse errorResponse)
            {
                HandlerError(errorResponse);
                return;
            }
                
            var downloadFileResponseresponse = (DownloadFileResponse)response;

            var localFileName = $"Files{Path.DirectorySeparatorChar}{downloadFileResponseresponse.Filename}";

            await using (var fileStream = File.OpenWrite(localFileName))
            await using (var binaryWriter = new BinaryWriter(fileStream))
            {
                binaryWriter.Write(downloadFileResponseresponse.File);
                binaryWriter.Flush();
            }

            stopwatch.Stop();
            Console.WriteLine($"File successfully downloaded! " +
                $"Average upload speed is {((double)downloadFileResponseresponse.FileSize / (1024 * 1024)) / (((double)stopwatch.ElapsedMilliseconds + 1) / 1000)} Mbps.");
        }

        public void Execute(DisconnectCommand _)
        {
            if (!_clientConnectionManager.IsConnected)
            {
                Console.WriteLine("Doesnt connected!");
            }
            else
            {
                _clientConnectionManager.Disconnect();
            }
        }

        private void HandlerError(ErrorResponse errorResponse)
        {
            Console.WriteLine("Server:" + errorResponse.ErrorText);
        }
    }
}

