using System;
using System.Linq;

namespace NetworkSocketServer.TransportLayer.Core.Buffer
{
    public class ArrayBuffer : IBuffer
    {
        private byte[] _buffer;

        public void Dispose()
        {
            _buffer = null;
        }

        public void Insert(byte[] array, int position)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
                
            _buffer = _buffer.Take(position)
                .Concat(array)
                .Concat(_buffer.Skip(position + array.Length))
                .ToArray();
        }

        public byte[] GetAll()
        {
            return _buffer;
        }

        public byte[] Get(int offset, int size)
        {
            var arraySegment = new ArraySegment<byte>(_buffer, offset, size);

            return arraySegment.ToArray();
        }

        public void Reinitialize(int length)
        {
            _buffer = new byte[length];
        }

        public int Length => _buffer.Length;
    }
}
