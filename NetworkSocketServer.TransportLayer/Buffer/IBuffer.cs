using System;

namespace NetworkSocketServer.TransportLayer.Buffer
{
    interface IBuffer : IDisposable
    {
        void Append(byte[] array);

        byte[] GetAll();

        byte[] Get(int length);

        byte[] Get(int length, int position);

        void SetLength(int length);

        void Clear();

        int Length { get; }
    }
}
