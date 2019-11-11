using System;

namespace NetworkSocketServer.Client.Command.Exceptions
{
    public class CommandNotFoundException : Exception
    {
        public CommandNotFoundException(string message) : base(message)
        {
        }
    }
}
