using System;

namespace NetworkSocketServer.DTO.Responses
{
    [Serializable]
    public class DateResponse : Response
    {
        public DateTime ServerTime { get; set; }

        public TimeSpan Offset { get; set; }
    }
}
