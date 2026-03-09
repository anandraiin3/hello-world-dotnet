using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace hello_world_dotnet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(ConfigureWebHost);

        public static void ConfigureWebHost(IWebHostBuilder webBuilder)
        {
            // Respect PORT env var (set automatically by Cloud Run) with fallback to 8080.
            // ASPNETCORE_URLS env var also works and takes precedence over this.
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            webBuilder
                .UseStartup<Startup>()
                .UseUrls($"http://+:{port}");
        }
    }
}
