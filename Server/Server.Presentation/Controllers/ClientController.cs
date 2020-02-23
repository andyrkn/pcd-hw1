using Server.Business;
using Server.Business.Monitor;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Presentation.Controllers
{
    public sealed class ClientController : BaseController
    {
        private readonly ITransferService transferService;

        public ClientController(IClientMonitor monitor, ITransferService transferService) :base(monitor)
        {
            this.transferService = transferService;
        }

        public async override Task SendBytesWithoutCheck(NetworkStream stream, int transferSize, int chunkSize, ClientModel clientModel)
        {
            await transferService.TransferWithNoCheck(stream, transferSize, chunkSize, clientModel);
        }

        public async override Task SendBytesWithCheck(NetworkStream stream, int transferSize, int chunkSize, ClientModel clientModel)
        {
            await transferService.TransferWithCheck(stream, transferSize, chunkSize, clientModel);
        }
    }
}
