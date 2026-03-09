using System;
using Microsoft.AspNetCore.Hosting;
using hello_world_dotnet;
using Xunit;

namespace hello_world_dotnet.Tests
{
    public class HelloWorldTests
    {
        [Fact]
        public void ConfigureWebHost_UsesPortEnvVar_DefaultsTo8080()
        {
            Environment.SetEnvironmentVariable("PORT", null);
            var webHostBuilder = new WebHostBuilder();

            Program.ConfigureWebHost(webHostBuilder);

            var serverUrls = webHostBuilder.GetSetting(WebHostDefaults.ServerUrlsKey);
            Assert.Contains(":8080", serverUrls);
        }

        [Fact]
        public void ConfigureWebHost_RespectsPortEnvVar()
        {
            Environment.SetEnvironmentVariable("PORT", "9090");
            var webHostBuilder = new WebHostBuilder();

            Program.ConfigureWebHost(webHostBuilder);

            var serverUrls = webHostBuilder.GetSetting(WebHostDefaults.ServerUrlsKey);
            Assert.Contains(":9090", serverUrls);
            Environment.SetEnvironmentVariable("PORT", null);
        }

        [Fact]
        public void Message_CanBeSetAndRetrieved()
        {
            var model = new HelloWorld
            {
                Message = "Test message"
            };

            Assert.Equal("Test message", model.Message);
        }

        [Fact]
        public void Timestamp_CanBeSetAndRetrieved()
        {
            var expected = new DateTime(2025, 3, 1, 12, 0, 0, DateTimeKind.Utc);
            var model = new HelloWorld
            {
                Timestamp = expected
            };

            Assert.Equal(expected, model.Timestamp);
        }

        [Fact]
        public void HelloWorld_DefaultValues_AreNullOrDefault()
        {
            var model = new HelloWorld();

            Assert.Null(model.Message);
            Assert.Equal(default(DateTime), model.Timestamp);
        }
    }
}
