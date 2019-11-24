using System;

namespace NetworkSocketServer.Commands
{
    [Serializable]
    public class TextCommand : Command
    {
        public string Text { get; set; }
    }
}
