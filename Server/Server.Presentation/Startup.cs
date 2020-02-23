using Microsoft.Extensions.DependencyInjection;
using Server.Business;
using Server.Presentation.Controllers;

namespace Server.Presentation
{
    internal sealed class Startup
    {
        public IServiceCollection Configure(IServiceCollection services)
        {
            services.AddTransient(typeof(ClientController));
            services.AddTransient<ITransferService, TransferService>();

            return services;
        }
    }
}
