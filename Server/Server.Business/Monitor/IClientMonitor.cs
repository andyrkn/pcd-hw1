using Server.Business.Monitor;
using System;
using System.Net.Sockets;

namespace Server.Business
{
    public interface IClientMonitor
    {
        public ClientModel AddClient(Socket client);

        public void RemoveClient(Guid id);

        public void UpdateStatus(Guid id, int done, double speed);

        public void SetClientInfo(Guid id, int transferSize, int chunkSize);
    }
}
