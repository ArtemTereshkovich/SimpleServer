using System;
using System.Threading.Tasks;

namespace NetworkSocketServer.Client.Commands
{
    public class DateCommand : ICommand
    {
        public const string Command = "-date";

        public DateTime ClientDateTime { get; }

        private DateCommand()
        {
            ClientDateTime = DateTime.Now;
            
        }

        public static DateCommand Parse(string _)
        {
            return new DateCommand();
        }

        public Task Execute(CommandExecutor executor)
        {
            executor.Execute(this);

            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return $"{Command}";
        }
    }
}
