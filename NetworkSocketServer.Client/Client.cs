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

        public string ClientId { get; set; }

        public Client(string clientId)
        {
            ClientId = clientId;

            _inputManager = new InputManager()
            {
                CommandParser = new CommandParser()
            };
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

                    command.Execute(executor);
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
