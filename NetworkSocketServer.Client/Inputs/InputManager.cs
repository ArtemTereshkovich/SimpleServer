using System;
using System.IO;
using NetworkSocketServer.Client.Commands;

namespace NetworkSocketServer.Client.Inputs
{
    public class InputManager
    {
        private readonly TextReader _reader;
        private readonly ICommandParser _commandParser;

        public InputManager(ICommandParser commandParser)
        {
            _reader = Console.In;
            _commandParser = commandParser;
        }

        public ICommand GetCommand()
        {
            var command = _reader.ReadLine();
            return _commandParser.Parse(command);
        }
    }
}
