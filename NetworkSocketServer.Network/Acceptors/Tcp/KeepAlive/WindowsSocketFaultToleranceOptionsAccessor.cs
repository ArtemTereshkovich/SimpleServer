using System;
using System.Net.Sockets;
using NetworkSocketServer.NetworkLayer.SocketOptionsAccessor;

namespace NetworkSocketServer.NetworkLayer.Tcp.KeepAlive
{
    internal class WindowsSocketFaultToleranceOptionsAccessor : ISocketOptionsAccessor
    {
        private readonly SocketFaultToleranceOptions _faultToleranceOptions;

        public WindowsSocketFaultToleranceOptionsAccessor(SocketFaultToleranceOptions faultToleranceOptions)
        {
            _faultToleranceOptions = faultToleranceOptions;
        }

        public void SetOptions(Socket tcpSocket)
        {
            if (tcpSocket.SocketType != SocketType.Stream)
            {
                throw new ArgumentException(nameof(tcpSocket));
            }

            tcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
            tcpSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, _faultToleranceOptions.KeepAliveTime);
            tcpSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, _faultToleranceOptions.KeepAliveInterval);

            #region HardSet
            
            //const int on = 1;
            //const uint keepAliveInterval = 10000; //Send a packet once every 10 seconds.
            //const uint retryInterval = 1000; //If no response, resend every second.

            //const int size = sizeof(uint);
            //var inArray = new byte[size * 3];
            //Array.Copy(BitConverter.GetBytes(@on), 0, inArray, 0, size);
            //Array.Copy(BitConverter.GetBytes(keepAliveInterval), 0, inArray, size, size);
            //Array.Copy(BitConverter.GetBytes(retryInterval), 0, inArray, size * 2, size);
            //tcpSocket.IOControl(IOControlCode.KeepAliveValues, inArray, null);

            #endregion
        }
    }
}
