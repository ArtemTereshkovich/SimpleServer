using System;

namespace NetworkSocketServer.TransportLayer.Buffer
{
    interface IBuffer : IDisposable
    {
        void Append(byte[] array);

        void Insert(byte[] array, int position);

        byte[] Get(int length);

        byte[] Get(int length, int position);

        void Remaining(int length);

        void Clear();
    }
}
