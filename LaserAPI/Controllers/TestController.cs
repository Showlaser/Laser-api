using LaserAPI.Logic;
using LaserAPI.Models.Helper.Laser;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace LaserAPI.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Send()
        {
            const int durationMs = 50000;
            const int delayTime = 10000;
            int iterations = 0;

            Stopwatch stopwatch = Stopwatch.StartNew();

            while (stopwatch.ElapsedMilliseconds < durationMs)
            {
                iterations++;

                await LaserConnectionLogic.SendMessage(new LaserMessage
                {
                    RedLaser = 2,
                    BlueLaser = 6,
                    GreenLaser = 7,
                    X = 2000,
                    Y = 0
                });
                Thread.SpinWait(delayTime);
                await LaserConnectionLogic.SendMessage(new LaserMessage
                {
                    RedLaser = 2,
                    BlueLaser = 6,
                    GreenLaser = 7,
                    X = -2000,
                    Y = 0
                });
                Thread.SpinWait(delayTime);
            }

            stopwatch.Stop();
            return $"{iterations} iterations";
        }
    }
}
