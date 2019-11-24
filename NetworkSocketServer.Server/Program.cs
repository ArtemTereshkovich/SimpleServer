using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using NetworkSocketServer.NetworkLayer.Acceptors.Tcp;
using NetworkSocketServer.NetworkLayer.Server;
using NetworkSocketServer.NetworkLayer.ServerBuilder;
using NetworkSocketServer.NetworkLayer.Tcp;
using NetworkSocketServer.NetworkLayer.Tcp.KeepAlive;
using NetworkSocketServer.NetworkLayer.TransportHandler;

namespace NetworkSocketServer.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var address = GetAddress();
            
            Console.WriteLine(address.ToString());

            var host = new SimpleServerBuilder(new EndPointServiceHandler())
                .WithTcpFaultToleranceAcceptor(
                    new TcpNetworkAcceptorSettings
                    {
                        ListenIpAddress = address,
                        ListenMaxBacklogConnection = 1,

                        ListenPort = 1337,
                    },
                    new SocketFaultToleranceOptions
                    {
                        KeepAliveInterval = 28000,
                        KeepAliveTime = 28000,
                    })
                .Build();

            host.StartHost();

            host.StopHost();
        }

        private static IPAddress GetAddress()
        {
            var firstUpInterface = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(IsAddressSuitable);

            if (firstUpInterface != null)
            {
                IPInterfaceProperties props = firstUpInterface.GetIPProperties();
                IPAddress firstIpV4Address = props.UnicastAddresses
                    .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Select(c => c.Address)
                    .FirstOrDefault();

                if (firstIpV4Address != null)
                {
                    return firstIpV4Address;
                }
            }

            throw new InvalidOperationException("There are no InterNetwork addresses set");
        }

        private static bool IsAddressSuitable(NetworkInterface networkInterface)
        {
            return networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                   networkInterface.OperationalStatus == OperationalStatus.Up;
        }
    }
}
