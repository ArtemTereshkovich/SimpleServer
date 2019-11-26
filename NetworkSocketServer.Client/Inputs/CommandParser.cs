using System.Linq;
using NetworkSocketServer.Client.Commands;
using NetworkSocketServer.Client.Commands.Exceptions;

namespace NetworkSocketServer.Client.Inputs
{
    public class CommandParser : ICommandParser
    {
        public ICommand Parse(string command)
        {
            var type = command.Split(' ').First();
            var data = string.Join(' ', command.Split(' ').Skip(1));

            switch (type)
            {
                case HelpCommand.Command:
                    {
                        return HelpCommand.Parse(data);
                    }
                case TextCommand.Command:
                    {
                        return TextCommand.Parse(data);
                    }
                case DateCommand.Command:
                    {
                        return DateCommand.Parse(data);
                    }
                case UploadFileCommand.Command:
                    {
                        return UploadFileCommand.Parse(data);
                    }
                case DownloadFileCommand.Command:
                    {
                        return DownloadFileCommand.Parse(data);
                    }
                case ConnectTCPCommand.Command:
                    {
                        return ConnectTCPCommand.Parse(data);
                    }
                case ConnectUDPCommand.Command:
                    {
                        return ConnectTCPCommand.Parse(data);
                    }
                case DisconnectCommand.Command:
                    {
                        return DisconnectCommand.Parse(data);
                    }
                default: throw new CommandNotFoundException($"Unsupported command: {command}");
                }
        }
    }
}
