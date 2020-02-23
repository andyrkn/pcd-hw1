using System.Threading.Tasks;

namespace Server.Presentation
{
    internal sealed class Program
    {
        static async Task Main() 
            => await new WebHostBuilder()
                .AddConfiguration("appsettings.json")
                .Build()
                .Start();
    }
}
