using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace hello_world_dotnet
{
    public class Program
    {
        public const string HttpBindingUrl = "http://+:80";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(ConfigureWebHost);

        public static void ConfigureWebHost(IWebHostBuilder webBuilder)
        {
            webBuilder
                .UseStartup<Startup>()
                .UseUrls(HttpBindingUrl);
        }
    }
}
