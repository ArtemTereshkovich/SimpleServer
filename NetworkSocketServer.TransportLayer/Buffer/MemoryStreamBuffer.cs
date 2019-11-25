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
            if(array == null || array.Length == 0)
                throw new ArgumentException(nameof(array));

            _memoryStream.Write(array,0, array.Length);
        }
        
        public byte[] GetAll()
        {
            return _memoryStream.ToArray();
        }

        public byte[] Get(int length)
        {
            if (length <= 0)
                throw new ArgumentException(nameof(length));

            var array = new byte[length];
            _memoryStream.Read(array, 0, length);

            return array;
        }

        public byte[] Get(int length, int position)
        {
            if (length <= 0 || position <= 0)
                throw new ArgumentException(nameof(length));


            var array = new byte[length];
            _memoryStream.Seek(0, SeekOrigin.Begin);
            _memoryStream.Read(array, position, length);

            return array;
        }

        public void SetLength(int length)
        {
            if (length <= 0)
                throw new ArgumentException(nameof(length));

            _memoryStream.SetLength(length);
        }

        public void Clear()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);
            _memoryStream.SetLength(0);
        }

        public int Length => (int) _memoryStream.Length;

        public void Dispose()
        {
            _memoryStream?.Dispose();
        }
    }
}
