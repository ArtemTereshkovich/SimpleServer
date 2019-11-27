using System;

namespace NetworkSocketServer.TransportLayer.Buffer
{
    public interface IBuffer : IDisposable
    {
        void Insert(byte[] array, int position);

        byte[] GetAll();

        byte[] Get(int position, int offset);

        void SetLength(int length);

        int Length { get; }
    }
}
