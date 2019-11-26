using System;
using System.Runtime.InteropServices;

namespace NetworkSocketServer.NetworkLayer.SocketOptionsAccessor.KeepAlive
{
    internal class PlatformBasedKeepAliveAccessorFactory : ISocketOptionsAccessorFactory
    {
        private readonly SocketKeepAliveOptions _socketKeepAliveOptions;

        public PlatformBasedKeepAliveAccessorFactory(SocketKeepAliveOptions socketKeepAliveOptions)
        {
            _socketKeepAliveOptions = socketKeepAliveOptions;
        }

        public ISocketOptionsAccessor GetSocketOptionsAccessor()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsSocketKeepAliveOptionsAccessor(_socketKeepAliveOptions);
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new LinuxSocketKeepAliveOptionsAccessor(_socketKeepAliveOptions);
            }

            throw new PlatformNotSupportedException();
        }
    }
}
