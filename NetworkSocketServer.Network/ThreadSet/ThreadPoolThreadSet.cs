using System;
using System.Threading;
using NetworkSocketServer.NetworkLayer.Acceptors;

namespace NetworkSocketServer.NetworkLayer.ThreadSet
{
    internal class ThreadPoolThreadSet : IThreadSet
    {
        public ThreadPoolThreadSet(int threadNumbers)
        {
            if(threadNumbers < 1)
                throw new ArgumentException(nameof(threadNumbers));

            ThreadPool.SetMinThreads(1, 1);

            ThreadPool.SetMaxThreads(threadNumbers, threadNumbers);
        }

        public void Execute(Action<INetworkAcceptor> action, INetworkAcceptor acceptor)
        {
            ThreadPool.QueueUserWorkItem(action, acceptor,false);
        }
    }
}
