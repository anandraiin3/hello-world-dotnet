using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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

        // Intentionally vulnerable examples for security scanners like Snyk.
        // These endpoints are not used by the normal application flow.

        [HttpGet("insecure-sql")]
        public IActionResult InsecureSql([FromQuery] string user)
        {
            // Hard-coded connection string with password (credential exposure)
            var connectionString = "Server=localhost;Database=Demo;User Id=sa;Password=P@ssw0rd123!;";

            // SQL query built via string concatenation (SQL injection)
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            var query = "SELECT * FROM Users WHERE Name = '" + user + "'";
            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            return Ok("Query executed");
        }

        [HttpPost("insecure-command")]
        public IActionResult InsecureCommand([FromBody] string command)
        {
            // Command injection via unvalidated shell command
            Process.Start("/bin/sh", "-c " + command);
            return Ok("Command started");
        }

        [HttpGet("insecure-file")]
        public IActionResult InsecureFile([FromQuery] string fileName)
        {
            // Path traversal by concatenating user input into a file path
            var path = "/var/data/" + fileName;
            var content = System.IO.File.ReadAllText(path);
            return Ok(content);
        }
    }
}