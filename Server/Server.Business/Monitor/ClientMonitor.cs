using Server.Business.Monitor;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace Server.Business
{
    public class ClientMonitor : IClientMonitor
    {
        private List<ClientModel> clients = new List<ClientModel>();
        private Timer timer;

        public ClientMonitor()
        {
            timer = new Timer();
            timer.Elapsed += Timer_Tick;
            timer.Interval = 500;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public ClientModel AddClient(Socket client)
        {
            var c = ClientFromSocket(client);
            clients.Add(c);
            return c;
        }

        public void RemoveClient(Guid id)
        {
            clients.Remove(clients.Find(c => c.id == id));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            sb.Append("Connected clients:\n");
            for(int i = 0; i < clients.Count; i++)
            {
                sb.Append($"{clients[i].clientIp}:{clients[i].clientPort} ; Speed: {clients[i].Speed} MB/s; Done: {clients[i].Done}%; TSize:{clients[i].TransferSize}; CSize:{clients[i].ChunkSize}\n");
            }

            Console.Clear();
            Console.Write(sb.ToString());
        }

        private ClientModel ClientFromSocket(Socket socket) =>
            new ClientModel
            {
                clientIp = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString(),
                clientPort = ((IPEndPoint)socket.RemoteEndPoint).Port.ToString()
            };

        public void UpdateStatus(Guid id, int done, double speed)
        {
            clients.Find(c => c.id == id).UpdateStatus(done, speed);
        }

        public void SetClientInfo(Guid id, int transferSize, int chunkSize)
        {
            clients.Find(c => c.id == id).SetInfo(transferSize, chunkSize);
        }
    }
}
