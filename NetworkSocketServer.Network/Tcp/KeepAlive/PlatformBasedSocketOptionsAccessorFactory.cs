using System;
using System.Runtime.InteropServices;

namespace NetworkSocketServer.Network.Tcp.KeepAlive
{
    internal class PlatformBasedSocketOptionsAccessorFactory : ISocketOptionsAccessorFactory
    {
        public ISocketOptionsAccessor GetSocketOptionsAccessor()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsSocketOptionsAccessor();
            }

            throw new PlatformNotSupportedException();
        }
    }
}
