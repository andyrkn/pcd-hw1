using Newtonsoft.Json;
using Server.Business;
using Server.Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Presentation.CoreWebHost
{
    public sealed class TcpWebHost : WebHost
    {
        private readonly TcpListener listener;

        public TcpWebHost(IServiceProvider provider, TcpListener listener) : base(provider)
        {
            this.listener = listener;
        }

        public override async Task Start()
        {
            listener.Start();
            
            while(true)
            {
                var client = await listener.AcceptTcpClientAsync();
                new Thread(new ThreadStart(async () =>
                {
                    var controller = (BaseController)provider.GetService(typeof(ClientController));
                    await controller.Route(client);
                })).Start();
            }
        }
    }
}
