using Server.Presentation.Constants;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Presentation.Controllers
{
    public abstract class BaseController
    {
        protected TcpClient client;
        public void Route(TcpClient client)
        {
            this.client = client;

            var stream = this.client.GetStream();
            var buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);

            ResolveController(stream, Encoding.UTF8.GetString(buffer));
        }

        public abstract void SendBytesWithoutCheck(NetworkStream stream, int transferSize);

        private void ResolveController(NetworkStream stream, string command)
        {
            char controller = command[0];
            int transferSize = TransferSizes.GetTransferSize(command[1].ToString());

            if(controller == '1')
            {
                SendBytesWithoutCheck(stream, transferSize);
            }
        }
    }
}
