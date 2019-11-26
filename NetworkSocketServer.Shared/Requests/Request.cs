using System;

namespace NetworkSocketServer.DTO.Requests
{
    [Serializable]
    public class Request
    {
        public Guid RequestId { get; set; }
    }
}
