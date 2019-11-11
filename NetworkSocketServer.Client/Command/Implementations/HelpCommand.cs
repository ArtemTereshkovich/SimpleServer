using System;
using System.Collections.Generic;
using System.Text;
using NetworkSocketServer.Client;

namespace SPOLKS.Client.Command
{
    public class HelpCommand : Implementations.Command
    {
        public const string Command = "-help";

        public override void Execute(CommandExecutor executor)
        {
            executor.Execute(this);
        }

        public static HelpCommand Parse(string _)
        {
            return new HelpCommand();
        }
    }
}
