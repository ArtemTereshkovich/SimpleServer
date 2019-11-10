using System;
using System.Net;
using System.Threading.Tasks;
using NetworkSocketsServer.Shared;

namespace NetworkSocketServer.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var settings = 
                new TcpNetworkAcceptorSettings
                {
                    ListenIpAddress = IPAddress.Parse("192.168.100.10"),
                    ListenPort = 4987,
                    ListenMaxBacklogConnection = 3,
                };

            var acceptor = new TcpKeepAliveNetworkAcceptor(settings);

            acceptor.StartListen();

            while (true)
            {
                if (acceptor.IsHaveNewConnection())
                {
                    var connection = await acceptor.AcceptConnection();
                    Console.WriteLine("Socket connected");
                }

                await Task.Delay(1500);
            }
        }
    }
}
