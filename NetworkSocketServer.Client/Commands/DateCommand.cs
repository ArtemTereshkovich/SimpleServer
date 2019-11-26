using System;
using System.Threading.Tasks;

namespace NetworkSocketServer.Client.Commands
{
    public class DateCommand : ICommand
    {
        public const string Command = "-time";

        public DateTime ClientDateTime { get; }

        private DateCommand()
        {
            ClientDateTime = DateTime.Now;
            
        }

        public static DateCommand Parse(string _)
        {
            return new DateCommand();
        }

        public async Task Execute(CommandExecutor executor)
        {
            await executor.Execute(this);
        }

        public override string ToString()
        {
            return $"{Command}";
        }
    }
}
