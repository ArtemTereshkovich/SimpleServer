using System;
using System.Threading.Tasks;

namespace NetworkSocketServer.TransportLayer.MessageHandler
{
    interface IMessageHandler : IDisposable
    {
        Guid ClientId { get; }

        Task Send(byte[] array);

        Task<byte> ReceiveMessage();
        

        void CloseHandler();
    }
}
