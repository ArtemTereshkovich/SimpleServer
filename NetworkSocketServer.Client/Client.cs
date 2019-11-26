using System;
using System.Net.Sockets;
using NetworkSocketServer.Client.Command.Exceptions;
using NetworkSocketServer.Client.Command.Implementations;
using NetworkSocketServer.Client.Command.Utils;

namespace NetworkSocketServer.Client
{
    public class Client
    {
        private InputManager _inputManager;
        private CommandExecutor _commandExecutor;

        public Client()
        {
            _inputManager = new InputManager()
            {
                CommandParser = new CommandParser()
            };

            _commandExecutor = new CommandExecutor();
        }

        public void Run()
        {
            var executor = new CommandExecutor();
            executor.ClientId = ClientId;

            while (true)
            {
                try
                {
                    var command = _inputManager.GetCommand();

                    executer
                }
                catch (CommandNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                    executor.Connection = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
