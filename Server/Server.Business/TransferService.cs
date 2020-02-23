using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Business
{
    public class TransferService : ITransferService
    {
        private readonly int OneMb = 1048576;
        public async void TransferWithNoCheck(NetworkStream networkStream, int transferSize)
        {
            var chunkSize = 1024;

            await WriteToStream(transferSize, chunkSize, async () =>
            {
                // var data = new string('1', chunkSize);
                var data = Generate(chunkSize);
                await networkStream.WriteAsync(Encoding.UTF8.GetBytes(data));
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
