using System.Threading.Tasks;

namespace NetworkSocketServer.NetworkLayer.Server
{
    public interface IServer
    {
        Task StartHost();

        void StopHost();
    }
}
