using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Server.Presentation;

namespace Client.Presentation
{
    public sealed class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var port = int.Parse(config.GetSection("port").Value);
            var chunkSize = config.GetSection("chunkSize").Value;
            var ip = config.GetSection("ip").Value;
            var tSize = int.Parse(args[0]);
            UdpSocket socket = new UdpSocket();
            socket.Start(ip, port, tSize, chunkSize);
        }
    }
}
