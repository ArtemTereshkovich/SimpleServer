using System;
using System.Threading.Tasks;
using NetworkSocketServer.Client.Commands;
using NetworkSocketServer.NetworkLayer.Connectors;
using NetworkSocketServer.NetworkLayer.Dispatchers.ConnectorDispatcher;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive;

namespace NetworkSocketServer.Client
{
    public class CommandExecutor
    {
        public CommandExecutor()
        {

        }

        public void Execute(HelpCommand _)
        {
            Console.WriteLine("Supported commands:");
            Console.WriteLine("-connecttcp [ip]:[port]");
            Console.WriteLine("-connectudp [ip]:[port]");
            Console.WriteLine("-disconnect");
            Console.WriteLine("-time");
            Console.WriteLine("-echo [message]");
            Console.WriteLine("-upload [file]");
            Console.WriteLine("-download [file]");
        }

        public async Task Execute(ConnectTCPCommand connectTcpCommand)
        {
            var dispatcherFactory = new ConnectorDispatcherFactory();

            var dispatcher = dispatcherFactory.CreateConnectorDispatcher(new SocketKeepAliveOptions
            {
                KeepAliveInterval = 90000,
                KeepAliveTime = 300000,
            });

            var transportHandler = await dispatcher.CreateTransportHandler(new NetworkConnectorSettings
            {
                ConnectionType = ConnectionType.Tcp,
                IpEndPointServer = connectTcpCommand.EndPoint
            });
        }

        public async Task Execute(ConnectUDPCommand connectUdpCommand)
        {
            var dispatcherFactory = new ConnectorDispatcherFactory();

            var dispatcher = dispatcherFactory.CreateConnectorDispatcher(new SocketKeepAliveOptions
            {
                KeepAliveInterval = 90000,
                KeepAliveTime = 300000,
            });

            var transportHandler = await dispatcher.CreateTransportHandler(new NetworkConnectorSettings
            {
                ConnectionType = ConnectionType.Udp,
                IpEndPointServer = connectUdpCommand.EndPoint
            });
        }

        public void Execute(TextCommand command)
        {
            //if (Connection == null)
            //{
            //    Console.WriteLine("There is no connection to server!");

            //    return;
            //}

            //var request = new TextRequest()
            //{
            //    CommandType = CommandType.EchoRequest,
            //    Text = command.Message
            //};

            //Connection.Send(request);
            //var response = Connection.Receive().Deserialize<TextRequest>();

            //Console.WriteLine(response.Text);
        }

        public void Execute(DateCommand _)
        {
            //if (Connection == null)
            //{
            //    Console.WriteLine("There is no connection to server!");

            //    return;
            //}

            //var request = new DateRequest()
            //{
            //    ClientDate = DateTime.Now,
            //};

            //Connection.Send(request);
            //var response = Connection.Receive().Deserialize<TextRequest>();

            //Console.WriteLine(response.Text);
        }
        public void Execute(UploadFileCommand fileCommand)
        {
            //if (Connection == null)
            //{
            //    Console.WriteLine("There is no connection to server!");

            //    return;
            //}

            //var localFileName = $"Resources{Path.DirectorySeparatorChar}{fileCommand.FileName}";
            //var fileInfo = new FileInfo(localFileName);

            //if (!File.Exists(localFileName))
            //{
            //    Console.WriteLine("File not found!");

            //    return;
            //}

            //var message = new UploadFileRequest()
            //{
            //    CommandType = CommandType.UploadFileRequest,
            //    ConnectionId = ClientId,
            //    FileName = fileCommand.FileName,
            //    Size = fileInfo.Length,
            //    IsExist = true
            //};

            //Connection.Send(message);
            //var serverFileInfoResponse = Connection.Receive().Deserialize<UploadFileRequest>();
          

            //var bytes = File.ReadAllBytes(localFileName).Skip((int) serverFileInfoResponse.Size).ToArray();

            //var stopwatch = new Stopwatch();
            //stopwatch.Restart();

            //Connection.Send(bytes);

            //Connection.Receive().Deserialize<Request>();
            //stopwatch.Stop();

            //Console.WriteLine($"File uploaded successfully! " +
            //    $"Average upload speed is {((double)bytes.Length / (1024 * 1024)) / (((double)stopwatch.ElapsedMilliseconds + 1) / 1000)} Mbps.");
        }
        public void Execute(DownloadFileCommand fileCommand)
        {
            //if (Connection == null)
            //{
            //    Console.WriteLine("There is no connection to server!");

            //    return;
            //}

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

        public void Execute(DisconnectCommand _)
        {
            //if (Connection == null)
            //{
            //    Console.WriteLine("There is no connection to server!");

            //    return;
            //}

            //Connection.Disconect();
            //Connection = null;

            //Console.WriteLine("Disconnected");
        }
    }
}

