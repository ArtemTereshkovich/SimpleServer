using System;

namespace NetworkSocketServer.DTO.Responses
{
    [Serializable]
    public class Response
    {
        public Guid ConnectionId { get; set; }
    }
}
