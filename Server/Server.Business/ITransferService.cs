using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server.Business
{
    public interface ITransferService
    {
        void TransferWithNoCheck(NetworkStream networkStream, int transferSize, int chunkSize);
        void TransferWithCheck(NetworkStream networkStream, int transferSize, int chunkSize);
    }
}
