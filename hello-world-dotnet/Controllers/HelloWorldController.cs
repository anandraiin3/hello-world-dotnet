using System;
using Microsoft.AspNetCore.Mvc;

namespace hello_world_dotnet.Controllers
{
    [ApiController]
    [Route("")]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public HelloWorld Get()
        {
            return new()
            {
                Message = "Automate all the things!",
                Timestamp = DateTime.Now
            };
        }
    }
}