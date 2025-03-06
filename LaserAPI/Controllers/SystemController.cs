using Microsoft.AspNetCore.Mvc;
using System;

namespace LaserAPI.Controllers
{
    [Route("system")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        [HttpGet("close")]
        public void Close()
        {
            Console.WriteLine("Closing command received. Shutting down.");
            Environment.Exit(0);
        }
    }
}
