using System;

namespace NetworkSocketServer.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientId = Guid.NewGuid().ToString();

            new Client(clientId).Run();
        }
    }
}
