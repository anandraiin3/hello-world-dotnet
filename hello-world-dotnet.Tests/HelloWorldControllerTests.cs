using System;
using hello_world_dotnet.Controllers;
using Xunit;

namespace hello_world_dotnet.Tests
{
    public class HelloWorldControllerTests
    {
        [Fact]
        public void Get_ReturnsHelloWorld_WithExpectedMessage()
        {
            var controller = new HelloWorldController();
            var result = controller.Get();

            Assert.NotNull(result);
            Assert.Equal("Automate all the things!", result.Message);
        }

        [Fact]
        public void Get_ReturnsHelloWorld_WithTimestampSet()
        {
            var controller = new HelloWorldController();
            var before = DateTime.UtcNow;
            var result = controller.Get();
            var after = DateTime.UtcNow;

            Assert.NotNull(result);
            Assert.True(result.Timestamp >= before.AddSeconds(-1) && result.Timestamp <= after.AddSeconds(1),
                $"Timestamp {result.Timestamp} should be between {before} and {after}");
        }

        [Fact]
        public void Get_ReturnsNonNullResult()
        {
            var controller = new HelloWorldController();
            var result = controller.Get();

            Assert.NotNull(result);
            Assert.NotNull(result.Message);
        }
    }
}
