using Server.Business;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Presentation.Controllers
{
    public sealed class ClientController : BaseController
    {
        private readonly ITransferService transferService;

        public ClientController(ITransferService transferService)
        {
            this.transferService = transferService;
        }

        public override void SendBytesWithoutCheck(NetworkStream stream, int transferSize, int chunkSize)
        {
            transferService.TransferWithNoCheck(stream, transferSize, chunkSize);
        }

        public override void SendBytesWithCheck(NetworkStream stream, int transferSize, int chunkSize)
        {
            transferService.TransferWithCheck(stream, transferSize, chunkSize);
        }
    }
}
