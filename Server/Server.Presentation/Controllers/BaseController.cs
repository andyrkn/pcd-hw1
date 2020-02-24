using Server.Business;
using Server.Business.Monitor;
using Server.Presentation.Constants;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Presentation.Controllers
{
    public abstract class BaseController
    {
        protected TcpClient client;
        private readonly IClientMonitor monitor;
        public BaseController(IClientMonitor monitor)
        {
            this.monitor = monitor;
        }
        public async Task Route(TcpClient client)
        {
            this.client = client;
            var clientModel = monitor.AddClient(this.client.Client);

            var stream = this.client.GetStream();
            var buffer = new byte[7];
            stream.Read(buffer, 0, buffer.Length);

            try
            {
                await ResolveController(stream, Encoding.ASCII.GetString(buffer), clientModel);
            }
            catch (Exception ex)
            {   }
            finally
            {
                monitor.RemoveClient(clientModel.id);
            }
        }

        private async Task ResolveController(NetworkStream stream, string command, ClientModel clientModel)
        {
            char controller = command[0];
            int transferSize = TransferSizes.GetTransferSize(command[1].ToString());
            int chunkSize = int.Parse(command.Remove(0, 2).Trim('_'));

            if(controller == '1')
            {
                await SendBytesWithoutCheck(stream, transferSize, chunkSize, clientModel);
            }
            else if (controller == '2')
            {
                await SendBytesWithCheck(stream, transferSize, chunkSize, clientModel);
            }
        }

        public abstract Task SendBytesWithoutCheck(NetworkStream stream, int transferSize, int chunkSize, ClientModel clientModel);
        public abstract Task SendBytesWithCheck(NetworkStream stream, int transferSize, int chunkSize, ClientModel clientModel);

    }
}
