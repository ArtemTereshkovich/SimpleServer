using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetworkSocketServer.Client.Commands;
using NetworkSocketServer.DTO.Requests;
using NetworkSocketServer.DTO.Responses;
using NetworkSocketServer.NetworkLayer.Connectors;
using NetworkSocketServer.TransportLayer.ServiceHandlers.NetworkClientManager;

namespace NetworkSocketServer.Client
{
    public class CommandExecutor
    {
        private readonly INetworkClientManager _networkClientManager;

        public CommandExecutor(INetworkClientManager networkClientManager)
        {
            _networkClientManager = networkClientManager;
        }

        public void Execute(HelpCommand _)
        {
            Console.WriteLine("Supported commands:");
            Console.WriteLine("-connecttcp [ip]:[port]");
            Console.WriteLine("-connectudp [ip]:[port]");
            Console.WriteLine("-disconnect");
            Console.WriteLine("-date");
            Console.WriteLine("-text [message]");
            Console.WriteLine("-upload [file]");
            Console.WriteLine("-download [file]");
        }

        public async Task Execute(ConnectTCPCommand connectTcpCommand)
        {
            if (_networkClientManager.IsConnected)
            {
                Console.WriteLine("Already connected!");
                return;
            }
            else
            {
                await _networkClientManager.Connect(new NetworkConnectorSettings
                {
                    ConnectionType = ConnectionType.Tcp,
                    IpEndPointServer = connectTcpCommand.EndPoint
                });
            }
        }

        public async Task Execute(ConnectUDPCommand connectUdpCommand)
        {
            if (_networkClientManager.IsConnected)
            {
                Console.WriteLine("Already connected!");
                return;
            }
            else
            {
                await _networkClientManager.Connect(new NetworkConnectorSettings
                {
                    ConnectionType = ConnectionType.Udp,
                    IpEndPointServer = connectUdpCommand.EndPoint
                });
            }
        }

        public async Task Execute(TextCommand command)
        {
            if (!_networkClientManager.IsConnected)
            {
                Console.WriteLine("Doesnt connected!");
                return;
            }

            var request = new TextRequest()
            {
                RequestId = Guid.NewGuid(),
                Text = command.Message
            };

            var response = await _networkClientManager.HandleRequest(request);

            var textResponse = response as TextResponse;

            Console.WriteLine($"Execute text command ({textResponse.ResponseId}):{textResponse.Text}");
        }

        public async Task Execute(DateCommand dateCommand)
        {
            if (!_networkClientManager.IsConnected)
            {
                Console.WriteLine("Doesnt connected!");
                return;
            }

            var request = new DateRequest
            {
                RequestId = Guid.NewGuid(),
                ClientDate = dateCommand.ClientDateTime
            };

            var response = await _networkClientManager.HandleRequest(request);

            var dateResponse = response as DateResponse;

            Console.WriteLine($"Execute date command ({dateResponse.ResponseId}):{dateResponse.ServerTime}. Time offset: {dateResponse.Offset}");
        }
        public async Task Execute(UploadFileCommand fileCommand)
        {
            if (!_networkClientManager.IsConnected)
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

            var response = await _networkClientManager.HandleRequest(request);

            var uploadResponse = response as UploadFileResponse;

            stopwatch.Stop();

            Console.WriteLine($"File uploaded successfully! {uploadResponse.ResponseId} " +
                $"Average upload speed is {((double)bytes.Length / (1024 * 1024)) / (((double)stopwatch.ElapsedMilliseconds + 1) / 1000)} Mbps.");
        }

        public async Task Execute(DownloadFileCommand fileCommand)
        {
            if (!_networkClientManager.IsConnected)
            {
                Console.WriteLine("Doesnt connected!");
                return;
            }

            var request = new DownloadFileRequest
            {
                RequestId = Guid.NewGuid(),
                Filename = fileCommand.FileName
            };

            var response = await _networkClientManager.HandleRequest(request);

            var downloadFileResponse = response as DownloadFileResponse;

            if (!downloadFileResponse.IsSuccess)
            {
                Console.WriteLine(downloadFileResponse.ErrorMessage);
            }
            else
            {

            }

            //var localFileName = $"Resources{Path.DirectorySeparatorChar}{fileCommand.FileName}";

            //var fileInfo = new FileInfo(localFileName);
            //var fileLength = fileInfo.Exists ? fileInfo.Length : 0;

            //var message = new UploadFileRequest()
            //{
            //    CommandType = CommandType.DownloadFileRequest,
            //    ConnectionId = ClientId,
            //    FileName = fileCommand.FileName,
            //    IsExist = fileInfo.Exists,
            //    Size = fileLength
            //};

            //Connection.Send(message);

            //var serverFileInfoResponse = Connection.Receive().Deserialize<UploadFileRequest>();
            //if (serverFileInfoResponse.FileName == null)
            //{
            //    Console.WriteLine("File not found!");
            //    return;
            //}

            //var stopwatch = new Stopwatch();
            //stopwatch.Restart();
            //Connection.Send(serverFileInfoResponse);

            //var bytesReceived = 0;
            //using (var fileStream = File.OpenWrite(localFileName))
            //using (var binaryWriter = new BinaryWriter(fileStream))
            //{
            //    binaryWriter.BaseStream.Seek(0, SeekOrigin.End);

            //    while (bytesReceived < serverFileInfoResponse.Size - fileLength)
            //    {
            //        var filePart = Connection.Receive();

            //        binaryWriter.Write(filePart);
            //        binaryWriter.Flush();

            //        bytesReceived += filePart.Length;
            //    }
            //}

            //stopwatch.Stop();
            //Console.WriteLine($"File successfully downloaded! " +
            //    $"Average upload speed is {((double)bytesReceived / (1024 * 1024)) / (((double)stopwatch.ElapsedMilliseconds + 1) / 1000)} Mbps.");
        }

        public async Task Execute(DisconnectCommand _)
        {
            if (!_networkClientManager.IsConnected)
            {
                Console.WriteLine("Doesnt connected!");
                return;
            }
            else
            {
                await _networkClientManager.Disconnect();
            }
        }
    }
}

