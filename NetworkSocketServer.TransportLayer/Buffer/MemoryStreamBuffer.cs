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


        public void Insert(byte[] array, int position)
        {
            throw new NotImplementedException();
        }

        public byte[] GetAll()
        {
            return _memoryStream.ToArray();
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

        public void SetLength(int lenght)
        {
            _memoryStream.SetLength(lenght);
        }

        public int Length => (int) _memoryStream.Length;

        public void Dispose()
        {
            _memoryStream?.Dispose();
        }
    }
}
