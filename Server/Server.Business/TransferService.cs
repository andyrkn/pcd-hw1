using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server.Business
{
    public class TransferService : ITransferService
    {
        private readonly int OneMb = 1048576;
        public async void TransferWithNoCheck(NetworkStream networkStream, int transferSize, int chunkSize)
        {

            await WriteToStream(transferSize, chunkSize, async () =>
            {
                var data = Generate(chunkSize);
                await networkStream.WriteAsync(Encoding.UTF8.GetBytes(data));
            });
        }
        public async void TransferWithCheck(NetworkStream networkStream, int transferSize, int chunkSize)
        {
            using var hasher = SHA256.Create();

            await WriteToStream(transferSize, chunkSize, async () =>
            {
                var accepted = false;
                var data = Generate(chunkSize);
                var dataBytes = Encoding.UTF8.GetBytes(data);
                var dataHash = hasher.ComputeHash(dataBytes);

                while (!accepted)
                {
                    // var data = new string('1', chunkSize);

                    await networkStream.WriteAsync(dataHash);
                    await networkStream.WriteAsync(dataBytes);

                    var buffer = new byte[2];

                    await networkStream.ReadAsync(buffer);
                    if(Encoding.UTF8.GetString(buffer) == "ok")
                    {
                        accepted = true;
                    }
                }
            });
        }

        public async Task WriteToStream(int transferSize, int chunkSize, Func<Task> writeAction)
        {
            for (int i = 0; i < transferSize; i++)
            {
                for (int j = 0; j < OneMb; j += chunkSize)
                {
                    await writeAction();
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
