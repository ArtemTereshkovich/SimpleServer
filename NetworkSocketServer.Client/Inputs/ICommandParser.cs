using NetworkSocketServer.Client.Commands;

namespace NetworkSocketServer.Client.Inputs
{
    public interface ICommandParser
    {
        ICommand Parse(string command);
    }
}
