using System;

namespace NetworkSocketServer.Client.Commands.Exceptions
{
    public class CommandNotFoundException : Exception
    {
        public CommandNotFoundException(string message) : base(message)
        {
        }
    }
}
