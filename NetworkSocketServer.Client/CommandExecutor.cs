using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NetworkSocketServer.Messages;
using SPOLKS.Client.Command;
using SPOLKS.Client.Command.Implementations;

namespace NetworkSocketServer.Client
{
    public class CommandExecutor
    {
        public TcpConnection Connection { get; set; }

        public string ClientId { get; set; }

        public void Execute(HelpCommand _)
        {
            Console.WriteLine("Supported commands:");
            Console.WriteLine("-connect [ip]:[port]");
            Console.WriteLine("-disconnect");
            Console.WriteLine("-time");
            Console.WriteLine("-echo [message]");
            Console.WriteLine("-upload [file]");
            Console.WriteLine("-download [file]");
        }
        public void Execute(EchoCommand command)
        {
            if (Connection == null)
            {
                Console.WriteLine("There is no connection to server!");

                return;
            }

            var request = new TextMessage()
            {
                MessageType = MessageType.EchoRequest,
                Text = command.Message
            };

            Connection.Send(request);
            var response = Connection.Receive().Deserialize<TextMessage>();

            Console.WriteLine(response.Text);
        }
        public void Execute(TimeCommand _)
        {
            if (Connection == null)
            {
                Console.WriteLine("There is no connection to server!");

                return;
            }

            var request = new Message()
            {
                MessageType = MessageType.TimeRequest
            };

            Connection.Send(request);
            var response = Connection.Receive().Deserialize<TextMessage>();

            Console.WriteLine(response.Text);
        }
        public void Execute(UploadCommand command)
        {
            if (Connection == null)
            {
                Console.WriteLine("There is no connection to server!");

                return;
            }

            var localFileName = $"Resources{Path.DirectorySeparatorChar}{command.FileName}";
            var fileInfo = new FileInfo(localFileName);

            if (!File.Exists(localFileName))
            {
                Console.WriteLine("File not found!");

                return;
            }

            var message = new FileInfoMessage()
            {
                MessageType = MessageType.UploadFileRequest,
                ClientId = ClientId,
                FileName = command.FileName,
                Size = fileInfo.Length,
                IsExist = true
            };

            Connection.Send(message);
            var serverFileInfoResponse = Connection.Receive().Deserialize<FileInfoMessage>();
          

            var bytes = File.ReadAllBytes(localFileName).Skip((int) serverFileInfoResponse.Size).ToArray();

            var stopwatch = new Stopwatch();
            stopwatch.Restart();

            Connection.Send(bytes);

            Connection.Receive().Deserialize<Message>();
            stopwatch.Stop();

            Console.WriteLine($"File uploaded successfully! " +
                $"Average upload speed is {((double)bytes.Length / (1024 * 1024)) / (((double)stopwatch.ElapsedMilliseconds + 1) / 1000)} Mbps.");
        }
        public void Execute(DownloadCommand command)
        {
            if (Connection == null)
            {
                Console.WriteLine("There is no connection to server!");

                return;
            }

            var localFileName = $"Resources{Path.DirectorySeparatorChar}{command.FileName}";

            var fileInfo = new FileInfo(localFileName);
            var fileLength = fileInfo.Exists ? fileInfo.Length : 0;

            var message = new FileInfoMessage()
            {
                MessageType = MessageType.DownloadFileRequest,
                ClientId = ClientId,
                FileName = command.FileName,
                IsExist = fileInfo.Exists,
                Size = fileLength
            };

            Connection.Send(message);

            var serverFileInfoResponse = Connection.Receive().Deserialize<FileInfoMessage>();
            if (serverFileInfoResponse.FileName == null)
            {
                Console.WriteLine("File not found!");
                return;
            }

            //if (serverFileInfoResponse.IsExist && serverFileInfoResponse.Size == fileLength)
            //{
            //    Console.WriteLine("File already downloaded!");
            //    return;
            //}

            // fix
            var stopwatch = new Stopwatch();
            stopwatch.Restart();
            Connection.Send(serverFileInfoResponse);

            var bytesReceived = 0;
            using (var fileStream = File.OpenWrite(localFileName))
            using (var binaryWriter = new BinaryWriter(fileStream))
            {
                binaryWriter.BaseStream.Seek(0, SeekOrigin.End);

                while (bytesReceived < serverFileInfoResponse.Size - fileLength)
                {
                    var filePart = Connection.Receive();

                    binaryWriter.Write(filePart);
                    binaryWriter.Flush();

                    bytesReceived += filePart.Length;
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"File successfully downloaded! " +
                $"Average upload speed is {((double)bytesReceived / (1024 * 1024)) / (((double)stopwatch.ElapsedMilliseconds + 1) / 1000)} Mbps.");
        }
        public void Execute(ConnectCommand command)
        {
            if (Connection != null && Connection.Connected)
            {
                Console.WriteLine("You already connected! Disconnect before start new connection!");

                return;
            }

            Connection = new TcpConnection();
            Connection.Connect(command.EndPoint);

            Console.WriteLine("Connected");
        }
        public void Execute(DisconnectCommand _)
        {
            if (Connection == null)
            {
                Console.WriteLine("There is no connection to server!");

                return;
            }

            Connection.Disconect();
            Connection = null;

            Console.WriteLine("Disconnected");
        }
    }
}

