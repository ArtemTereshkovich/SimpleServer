using System;
using System.IO;

namespace NetworkSocketServer.TransportLayer.Buffer
{
    class MemoryStreamBuffer : IBuffer
    {
        private readonly MemoryStream _memoryStream;

        public MemoryStreamBuffer()
        {
            _memoryStream = new MemoryStream();
        }
        
        public void Append(byte[] array)
        {
            _memoryStream.Write(array,0, array.Length);
        }

        public void Insert(byte[] array, int position)
        {
            _memoryStream.Write(array, position, array.Length);
        }

        public byte[] Get(int length)
        {
            var array = new byte[length];
            _memoryStream.Read(array, 0, length);

            return array;
        }

        public byte[] Get(int length, int position)
        {
            var array = new byte[length];
            _memoryStream.Read(array, position, length);

            return array;
        }

        public void Remaining(int length)
        {
            _memoryStream.Seek(length, SeekOrigin.Begin);
        }

        public void Clear()
        {
            _memoryStream.SetLength(0);
        }

        public void Dispose()
        {
            _memoryStream?.Dispose();
        }
    }
}
