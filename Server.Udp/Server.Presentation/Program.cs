using System;

namespace Server.Presentation
{
    public sealed class Program
    {
        static void Main()
        {
            var socket = new UdpSocket();

            socket.Start();
        }
    }
}
