using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Client.Business
{
    public sealed class Transfer
    {
        private readonly Socket socket;
        private readonly int OMB = 1048576;
        private int MB = 0;
        private int readBytes = 0;

        public Transfer(Socket socket)
        {
            this.socket = socket;
        }

        public void StartRead(int transferSize, int chunkSize)
        {
            //using var sw = new StreamWriter($"data_{transferSize}_{chunkSize}_{DateTime.Now:hh:mm:ss}");
            var timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < transferSize; i++)
            {
                for (int j = 0; j < OMB; j += chunkSize)
                {
                    var buffer = new byte[chunkSize];
                    readBytes += socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                    if (readBytes > OMB)
                    {
                        readBytes -= OMB;
                        MB += 1;
                    }
                }  
                Console.WriteLine(i);
            }

            var stats = $"{MB} {readBytes} {timer.Elapsed.TotalSeconds}";
            Console.WriteLine(stats);
        }
    }
}
