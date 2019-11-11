using System;
using System.Linq;
using System.Net;
using NetworkSocketServer.Client;

namespace SPOLKS.Client.Command
{
    public class ConnectCommand : Implementations.Command
    {
        public const string Command = "-connect";

        public IPEndPoint EndPoint { get; private set; }


        public override void Execute(CommandExecutor executor)
        {
            executor.Execute(this);
        }

        public static ConnectCommand Parse(string data)
        {
            var endPointData = data.Split(':').Select(value => value.TrimEnd(' ', '\0', '\n')).ToList();
            if (endPointData.Count != 2) throw new Exception("Wrong command format");

            var isIpAddressValid = IPAddress.TryParse(endPointData[0], out var address);
            if (!isIpAddressValid) throw new Exception("Wrong command format");

            var isPortValid = int.TryParse(endPointData[1], out var port);
            if (!isPortValid) throw new Exception("Wrong command format");

            return new ConnectCommand()
            {
                EndPoint = new IPEndPoint(address, port)
            };
        }
    }
}
