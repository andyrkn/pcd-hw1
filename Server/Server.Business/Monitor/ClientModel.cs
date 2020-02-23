using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Business.Monitor
{
    public sealed class ClientModel
    {
        public ClientModel()
        {
            id = Guid.NewGuid();
            TransferSize = 0;
            ChunkSize = 0;
            Done = 0;
        }

        public Guid id { get; set; }
        public string clientIp {get;set;}
        public string clientPort { get; set; }
        public int TransferSize { get; set; }
        public int ChunkSize { get; set; }
        public int Done { get; set; }

        public double Speed { get; set; }

        public void UpdateStatus(int done, double speed)
        {
            this.Done = done;
            this.Speed = speed;
        }

        public void SetInfo(int transferSize, int chunkSize)
        {
            this.TransferSize = transferSize;
            this.ChunkSize = chunkSize;
        }
    }
}
