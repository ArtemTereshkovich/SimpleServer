using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NetworkSocketServer.Client.Commands
{
    public class ConnectTCPCommand : ICommand
    {
        public const string Command = "-connecttcp";

        public IPEndPoint EndPoint { get; private set; }


        public async Task Execute(CommandExecutor executor)
        {
            await executor.Execute(this);
        }

        public static ConnectTCPCommand Parse(string data)
        {
            var endPointData = data.Split(':').Select(value => value.TrimEnd(' ', '\0', '\n')).ToList();
            if (endPointData.Count != 2) throw new Exception("Wrong command format");

            var isIpAddressValid = IPAddress.TryParse(endPointData[0], out var address);
            if (!isIpAddressValid) throw new Exception("Wrong command format");

            var isPortValid = int.TryParse(endPointData[1], out var port);
            if (!isPortValid) throw new Exception("Wrong command format");

            return new ConnectTCPCommand()
            {
                EndPoint = new IPEndPoint(address, port)
            };
        }
    }
}