using System;
using System.IO;
using NetworkSocketServer.Client.Command.Interfaces;

namespace NetworkSocketServer.Client.Command.Utils
{
    public class InputManager
    {
        public TextReader Reader { get; set; }
        public ICommandParser CommandParser {get;set; }

        public InputManager()
        {
            Reader = Console.In;
        }

        public ICommand GetCommand()
        {
            var command = Reader?.ReadLine();
            return CommandParser.Parse(command);
        }
    }
}
