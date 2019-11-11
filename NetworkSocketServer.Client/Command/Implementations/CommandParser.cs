using SPOLKS.Client.Command.Interfaces;
using System.Linq;
using NetworkSocketServer.Client.Command.Exceptions;
using SPOLKS.Client.Command.Implementations;

namespace SPOLKS.Client.Command
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
                case TimeCommand.Command:
                    {
                        return TimeCommand.Parse(data);
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
