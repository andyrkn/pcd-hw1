using Server.Business.Monitor;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Server.Business
{
    public class TransferService : ITransferService
    {
        private readonly int OneMb = 1048576;
        private readonly IClientMonitor clientMonitor;

        public TransferService(IClientMonitor clientMonitor)
        {
            this.clientMonitor = clientMonitor;
        }

        public async Task TransferWithNoCheck(NetworkStream networkStream, int transferSize, int chunkSize, ClientModel clientModel)
        {
            clientMonitor.SetClientInfo(clientModel.id, transferSize, chunkSize);

            await WriteToStream(transferSize, chunkSize, clientModel, async () =>
            {
                var data = Generate(chunkSize);
                await networkStream.WriteAsync(Encoding.ASCII.GetBytes(data));
                return chunkSize;
            });
        }
        public async Task TransferWithCheck(NetworkStream networkStream, int transferSize, int chunkSize, ClientModel clientModel)
        {
            clientMonitor.SetClientInfo(clientModel.id, transferSize, chunkSize);
            using var hasher = SHA256.Create();

            await WriteToStream(transferSize, chunkSize, clientModel, async () =>
            {
                var sent = 0;
                var accepted = false;
                var data = Generate(chunkSize);
                var dataBytes = Encoding.ASCII.GetBytes(data);
                var dataHash = hasher.ComputeHash(dataBytes);

                while (!accepted)
                {
                    networkStream.Write(dataHash, 0, dataHash.Length);
                    await networkStream.WriteAsync(dataBytes);

                    var buffer = new byte[2];
                    
                    await networkStream.ReadAsync(buffer);
                    sent += chunkSize + 34;
                    if (Encoding.ASCII.GetString(buffer) == "ok")
                    {
                        accepted = true;
                    }
                }

                return sent;
            });
        }

        public async Task WriteToStream(int transferSize, int chunkSize, ClientModel clientModel, Func<Task<int>> writeAction)
        {
            var sentData = 0;
            var watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < transferSize; i++)
            {
                for (int j = 0; j < OneMb; j += chunkSize)
                {
                    sentData += await writeAction();

                    if(watch.ElapsedMilliseconds > 500)
                    {
                        clientMonitor.UpdateStatus(clientModel.id, i * 100 / transferSize, Math.Round(sentData / (double)1048576 * 2, 3));
                        watch.Restart();
                        sentData = 0;
                    }
                }
            }
        }

        public string Generate(int chunkSize)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < chunkSize; i += 32)
            {
                sb.Append(Guid.NewGuid().ToString("n"));
            }

            return sb.ToString();
        }
    }
}
