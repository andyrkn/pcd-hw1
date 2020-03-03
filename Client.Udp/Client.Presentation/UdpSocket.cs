using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Client.Business;

namespace Server.Presentation
{
    public sealed class UdpSocket
    {
        private Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        public UdpSocket()
        {

        }

        public void Start(string ip, int port, int transferSize, string chunkSize)
        {
            var transferInfo = $"{transferSize}{chunkSize}";
            Socket.Connect(IPAddress.Parse(ip), port);

            var bytes = Encoding.ASCII.GetBytes(transferInfo);
            Socket.Send(bytes, SocketFlags.None);
            Console.WriteLine("Connected");
            new Transfer(Socket).StartRead(transferSize, int.Parse(chunkSize.Trim('_')));
        }

    }
}