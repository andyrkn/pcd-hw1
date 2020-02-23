using Server.Presentation.Constants;
using System.Net.Sockets;
using System.Text;

namespace Server.Presentation.Controllers
{
    public abstract class BaseController
    {
        protected TcpClient client;
        public void Route(TcpClient client)
        {
            this.client = client;

            var stream = this.client.GetStream();
            var buffer = new byte[7];
            stream.Read(buffer, 0, buffer.Length);

            ResolveController(stream, Encoding.UTF8.GetString(buffer));
        }

        private void ResolveController(NetworkStream stream, string command)
        {
            char controller = command[0];
            int transferSize = TransferSizes.GetTransferSize(command[1].ToString());
            int chunkSize = int.Parse(command.Remove(0, 2).Trim('_'));

            if(controller == '1')
            {
                SendBytesWithoutCheck(stream, transferSize, chunkSize);
            }

            if (controller == '2')
            {
                SendBytesWithCheck(stream, transferSize, chunkSize);
            }
        }

        public abstract void SendBytesWithoutCheck(NetworkStream stream, int transferSize, int chunkSize);
        public abstract void SendBytesWithCheck(NetworkStream stream, int transferSize, int chunkSize);

    }
}
