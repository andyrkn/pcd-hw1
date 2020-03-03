using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server.Business
{
    public class Transfer
    {
        private readonly int OMB = 1048576;
        private readonly Socket socket;

        public Transfer(Socket socket)
        {
            this.socket = socket;
        }

        public void StartSending(int transferSize, int chunkSize, EndPoint endPoint)
        {
            for (int i = 0; i < transferSize+1; i++)
            {
                for (int j = 0; j < OMB; j += chunkSize)
                {
                    byte[] data = Encoding.ASCII.GetBytes(Generate(chunkSize));
                    socket.SendTo(data, 0, data.Length, SocketFlags.None, endPoint);
                }
            }
        }

        private string Generate(int chunkSize)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return $"{new string(Enumerable.Repeat(chars,chunkSize-5).Select(s => s[random.Next(s.Length)]).ToArray())}<ACK>";
        }
    }
}
