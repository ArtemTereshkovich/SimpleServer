using System;
using System.Net.Sockets;
using NetworkSocketServer.Client.Commands.Exceptions;
using NetworkSocketServer.Client.Inputs;

namespace NetworkSocketServer.Client
{
    public class Client
    {
        private InputManager _inputManager;
        private CommandExecutor _commandExecutor;

        public Client()
        {
            _inputManager = new InputManager(new CommandParser());

            _commandExecutor = new CommandExecutor();
        }

        public void Run()
        {

            while (true)
            {
                try
                {
                    var command = _inputManager.GetCommand();

                    command.Execute(_commandExecutor);
                }
                catch (CommandNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
