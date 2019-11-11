namespace SPOLKS.Client.Command.Interfaces
{
    public interface ICommandParser
    {
        ICommand Parse(string command);
    }
}
