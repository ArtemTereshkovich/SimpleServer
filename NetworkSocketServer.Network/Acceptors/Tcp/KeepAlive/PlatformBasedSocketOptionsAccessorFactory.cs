using System;
using System.Runtime.InteropServices;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor;

namespace NetworkSocketServer.NetworkLayer.Tcp.KeepAlive
{
    internal class PlatformBasedSocketOptionsAccessorFactory : ISocketOptionsAccessorFactory
    {
        private readonly SocketFaultToleranceOptions _socketFaultToleranceOptions;

        public PlatformBasedSocketOptionsAccessorFactory(SocketFaultToleranceOptions socketFaultToleranceOptions)
        {
            _socketFaultToleranceOptions = socketFaultToleranceOptions;
        }

        public ISocketOptionsAccessor GetSocketOptionsAccessor()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsSocketFaultToleranceOptionsAccessor(_socketFaultToleranceOptions);
            }

            throw new PlatformNotSupportedException();
        }
    }
}
