using System.Threading.Tasks;

namespace NetworkSocketServer.Client.Commands
{
    public interface ICommand
    {
        Task Execute(CommandExecutor executor);
    }
}
