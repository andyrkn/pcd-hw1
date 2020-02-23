using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Net;
using Server.Presentation.CoreWebHost;

namespace Server.Presentation
{
    public sealed class WebHostBuilder
    {
        private IServiceCollection services;

        public WebHostBuilder()
        {
            services = new ServiceCollection();
        }

        public WebHostBuilder AddConfiguration(string fileName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(fileName, optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            services.AddSingleton(configuration);

            return this;
        }

        public TcpWebHost Build()
        {
            var startup = new Startup();
            services = startup.Configure(services);
            var provider = services.BuildServiceProvider();

            var port = provider.GetRequiredService<IConfigurationRoot>().GetSection("Startup")["port"];

            var listener = new TcpListener(IPAddress.Any, int.Parse(port));

            return new TcpWebHost(provider, listener);
        }
    }
}
