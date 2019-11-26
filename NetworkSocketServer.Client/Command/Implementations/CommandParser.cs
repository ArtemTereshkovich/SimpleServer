using System.Linq;
using NetworkSocketServer.Client.Command.Exceptions;
using NetworkSocketServer.Client.Command.Interfaces;
using SPOLKS.Client.Command.Implementations;

namespace NetworkSocketServer.Client.Command.Implementations
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
                case EchoCommand.Command:
                    {
                        return EchoCommand.Parse(data);
                    }
                case DateCommand.Command:
                    {
                        return DateCommand.Parse(data);
                    }
                case UploadCommand.Command:
                    {
                        return UploadCommand.Parse(data);
                    }
                case DownloadCommand.Command:
                    {
                        return DownloadCommand.Parse(data);
                    }
                case ConnectCommand.Command:
                    {
                        return ConnectCommand.Parse(data);
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
