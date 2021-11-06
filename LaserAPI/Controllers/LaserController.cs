using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace LaserAPI.Controllers
{
    [Route("laser")]
    [ApiController]
    public class LaserController : ControllerBase
    {
        private readonly LaserConnection _laserConnection;

        public LaserController(LaserConnection laserConnection)
        {
            _laserConnection = laserConnection;
        }

        [HttpGet("send")]
        public string SendMessage()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            int iterations = 0;
            int durationInSeconds = 1;

            while (stopwatch.ElapsedMilliseconds < durationInSeconds * 1000)
            {
                iterations++;
                _laserConnection.SendMessage("r5");
            }

            stopwatch.Stop();
            return $"{iterations} messages in {durationInSeconds} seconds";
        }

        [HttpPost]
        public void Post(int value)
        {
            for (int i = 0; i < 150000; i++)
            {
                _laserConnection.SendMessage($"r{value}");
            }
        }
    }
}
