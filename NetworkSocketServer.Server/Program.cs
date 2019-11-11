using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using NetworkSocketServer.Network.Host;
using NetworkSocketServer.Network.Tcp;
using NetworkSocketServer.Network.Tcp.KeepAlive;

namespace NetworkSocketServer.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new SimpleHostBuilder(
                    new EndPointServiceHandler(),
                    4)
                .WithTcpFaultToleranceAcceptor(
                    new TcpNetworkAcceptorSettings
                    {
                        ListenIpAddress = GetAddress(),
                        ListenMaxBacklogConnection = 1,
                        ListenPort = 13000,
                    },
                    new SocketFaultToleranceOptions
                    {
                        KeepAliveInterval = 30,
                        KeepAliveTime = 10,
                    }).Build();

            host.StartHost();
        }

        private static IPAddress GetAddress()
        {
            var firstUpInterface = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(IsAddressSuitable);

            if (firstUpInterface != null)
            {
                IPInterfaceProperties props = firstUpInterface.GetIPProperties();
                // get first IPV4 address assigned to this interface
                IPAddress firstIpV4Address = props.UnicastAddresses
                    .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Select(c => c.Address)
                    .FirstOrDefault();

                if (firstIpV4Address != null)
                {
                    Console.WriteLine(firstIpV4Address.ToString());
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
