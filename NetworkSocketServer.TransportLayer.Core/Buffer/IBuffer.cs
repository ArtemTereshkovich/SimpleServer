using System;

namespace NetworkSocketServer.TransportLayer.Core.Buffer
{
    public interface IBuffer : IDisposable
    {
        void Insert(byte[] array, int position);

        byte[] GetAll();

        byte[] Get(int offset, int size);

        void Reinitialize(int length);

        int Length { get; }
    }
}
