using Client.Business;
using Client.Config;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Presentation
{
    class Program
    {
        static async Task Main(string[] argv)
        {
            Console.Clear();
            var config = ClientConfiguration.GetConfig(Directory.GetCurrentDirectory());

            var adress = config.GetSection("Startup")["ip"];
            var port = config.GetSection("Startup")["port"];
            var chunk = config.GetSection("Startup")["chunkSize"];

            TcpClient client = new TcpClient();
            client.Connect(adress, int.Parse(port));

            var buffer = Encoding.ASCII.GetBytes($"{argv[0]}{chunk}");
            chunk = chunk.Trim('_');

            client.GetStream().Write(buffer, 0, buffer.Length);


            if (argv[0][0] == '1') await new Transfer().ReadWithoutCheck(client, TransferSizes.GetTransferSize(argv[0]), int.Parse(chunk));
            else                   await new Transfer().ReadWithCheck(client, TransferSizes.GetTransferSize(argv[0]), int.Parse(chunk));

            Console.WriteLine("Done");
            client.Client.Disconnect(true);
        }
    }
}
