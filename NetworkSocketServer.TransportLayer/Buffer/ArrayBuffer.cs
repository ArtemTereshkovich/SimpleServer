using System;
using System.Linq;

namespace NetworkSocketServer.TransportLayer.Buffer
{
    class ArrayBuffer : IBuffer
    {
        private byte[] _buffer;

        public void Dispose()
        {
            _buffer = null;
        }

        public void Insert(byte[] array, int position)
        {
            _buffer = _buffer.Take(position)
                .Concat(array)
                .Concat(_buffer.Skip(position + array.Length))
                .ToArray();
        }

        public byte[] GetAll()
        {
            return _buffer;
        }

        public byte[] Get(int length, int position)
        {
            var arraySegment = new ArraySegment<byte>(_buffer, position, length);

            return arraySegment.ToArray();
        }

        public void SetLength(int length)
        {
            _buffer = new byte[length];
        }

        public int Length => _buffer.Length;
    }
}
