using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Presentation.CoreWebHost
{
    public abstract class WebHost : IWebHost
    {
        protected readonly IServiceProvider provider;

        public WebHost(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public abstract Task Start();
    }
}
