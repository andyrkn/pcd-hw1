using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client.Business
{
    public sealed class Transfer
    {
        private readonly int OneMb = 1048576;
        private Stopwatch watch = new Stopwatch();
        private readonly Logger Logger = new Logger();
        public async Task ReadWithoutCheck(TcpClient client, int transferSize, int chunkSize)
        {
            Logger.Connected(transferSize, chunkSize);
            var stream = client.GetStream();

            await ReadFromStream(transferSize, chunkSize, async (i,j) =>
            {
                var data = new byte[chunkSize];
                var totalBytesOverNetwork = await stream.ReadAsync(data);

                return totalBytesOverNetwork;
            });
        }

        public async Task ReadWithCheck(TcpClient client, int transferSize, int chunkSize)
        {
            Logger.Connected(transferSize, chunkSize);
            var stream = client.GetStream();
            using var hasher = SHA256.Create();
            using var sw = new StreamWriter($"{DateTime.Now.ToString("hh-mm-ss")}rawFile");

            await ReadFromStream(transferSize, chunkSize, async (i,j) =>
            {
                var totalBytesOverNetwork = 0;
                var accepted = false;
                while (!accepted)
                {
                    var dataHash = new byte[32];
                    var data = new byte[chunkSize];

                    stream.Read(dataHash, 0, 32);         
                    totalBytesOverNetwork += 32;

                    await stream.ReadAsync(data, 0, chunkSize);

                    totalBytesOverNetwork += chunkSize;

                    var actualHash = hasher.ComputeHash(data);

                    if (((ReadOnlySpan<byte>)dataHash).SequenceEqual(actualHash))
                    {
                        await stream.WriteAsync(Encoding.ASCII.GetBytes("ok"));
                        await sw.WriteAsync(Encoding.ASCII.GetString(data));
                        totalBytesOverNetwork += 2;
                        accepted = true;
                    }
                    else
                    {
                        await stream.WriteAsync(Encoding.ASCII.GetBytes("no"));
                        Logger.LogError(i, j, chunkSize);
                    }
                }

                return totalBytesOverNetwork;
            });
        }

        public async Task ReadFromStream(int transferSize, int chunkSize, Func<int,int,Task<int>> readAction)
        {
            watch.Start();
            var data = 0;
            for (int i = 0; i < transferSize; i++)
            {
                for (int j = 0; j < OneMb; j += chunkSize)
                {
                    data += await readAction(i,j);
                    if (watch.ElapsedMilliseconds > 100)
                    {
                        Logger.LogSpeed(data, watch.ElapsedMilliseconds);
                        watch.Restart();
                        data = 0;
                    }
                }
                Logger.LogPercentage(i + 1, transferSize);
            }

            Logger.Dispose();
        }
    }
}
