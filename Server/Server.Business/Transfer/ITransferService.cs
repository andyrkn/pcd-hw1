using Server.Business.Monitor;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server.Business
{
    public interface ITransferService
    {
        Task TransferWithNoCheck(NetworkStream networkStream, int transferSize, int chunkSize, ClientModel clientModel);
        Task TransferWithCheck(NetworkStream networkStream, int transferSize, int chunkSize, ClientModel clientModel);
    }
}
