using System;

namespace NetworkSocketServer.DTO.Responses
{
    [Serializable]
    public class ErrorResponse : Response
    {
        public string ErrorText { get; set; }
    }
}
