using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

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
                Console.WriteLine($"{MB} {readBytes}");
            }

        }

        public void ReadCallback(IAsyncResult ar)
        {
            var s = (Socket)ar.AsyncState;
            var b = s.EndReceive(ar);

            readBytes += b;

            if (readBytes > OMB)
            {
                readBytes -= OMB;
                MB += 1;
            }
        }
    }
}
