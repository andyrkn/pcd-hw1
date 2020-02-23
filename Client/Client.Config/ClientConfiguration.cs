using Microsoft.Extensions.Configuration;

namespace Client.Config
{
    public sealed class ClientConfiguration
    {
        public static IConfigurationRoot GetConfig(string path) 
            => new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
    }
}
