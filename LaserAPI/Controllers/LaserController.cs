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
        public async Task<string> SendMessage()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            double previousMicroSeconds = 0.00;
            int iterations = 0;
            int durationInSeconds = 1;

            while (stopwatch.ElapsedMilliseconds < durationInSeconds * 1000)
            {
                double ticks = stopwatch.ElapsedTicks;
                double microSeconds = (ticks / Stopwatch.Frequency) * 1000000;

                if (microSeconds - previousMicroSeconds > 80)
                {
                    iterations++;
                    await _laserConnection.SendMessage("r5");
                    previousMicroSeconds = microSeconds;
                }
            }
            
            stopwatch.Stop();
            return $"{iterations} messages in {durationInSeconds} seconds";
        }

        [HttpPost]
        public async Task Post (int value)
        {
            await _laserConnection.SendMessage($"r{value}");
        }
    }
}
