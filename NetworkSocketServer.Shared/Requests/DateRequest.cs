using System;

namespace NetworkSocketServer.DTO.Requests
{
    [Serializable]
    public class DateRequest : Request
    {
        public DateTime ClientDate { get; set; }
    }
}
