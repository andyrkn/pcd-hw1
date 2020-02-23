using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Business
{
    public sealed class Transfer
    {
        private readonly int OneMb = 1048576;
        private Stopwatch watch = new Stopwatch();
        public async Task Start(TcpClient client, int transferSize, int chunkSize)
        {
            Logger.Connected();
            var stream = client.GetStream();

            using var sw = new StreamWriter(DateTime.Now.Ticks.ToString(), true);
            await ReadFromStream(transferSize, chunkSize, async () =>
            {
                // var data = new string('1', chunkSize);
                var data = new byte[chunkSize];
                var result = await stream.ReadAsync(data);
                sw.Write(Encoding.UTF8.GetString(data));
            });
        }

        public async Task ReadFromStream(int transferSize, int chunkSize, Func<Task> readAction)
        {
            watch.Start();
            var data = 0;
            for (int i = 0; i < transferSize; i++)
            {
                for (int j = 0; j < OneMb; j += chunkSize)
                {
                    await readAction();
                    data += chunkSize;
                    if(watch.ElapsedMilliseconds > 100)
                    {
                        Logger.LogSpeed(data);
                        watch.Restart();
                        data = 0;
                    }
                }
                Logger.LogPercentage(i, transferSize);
            }
        }

        public string Generate(int chunkSize)
        {
            var sb = new StringBuilder();

            for(int i = 0; i < chunkSize; i+=32)
            {
                sb.Append(Guid.NewGuid().ToString("n"));
            }

            return sb.ToString();
        }
    }
}
