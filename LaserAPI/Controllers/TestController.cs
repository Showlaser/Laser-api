using LaserAPI.Logic;
using LaserAPI.Models.Helper.Laser;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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
            const int durationMs = 4000;
            int iterations = 0;

            Stopwatch stopwatch = Stopwatch.StartNew();

            while (stopwatch.ElapsedMilliseconds < durationMs)
            {
                if (iterations < 1500)
                {
                    iterations++;
                }

                await LaserConnectionLogic.SendMessage(new LaserMessage
                {
                    RedLaser = 7,
                    BlueLaser = 0,
                    GreenLaser = 0,
                    X = -50 - iterations,
                    Y = 0
                });
                await LaserConnectionLogic.SendMessage(new LaserMessage
                {
                    RedLaser = 0,
                    BlueLaser = 7,
                    GreenLaser = 0,
                    X = 0,
                    Y = 50 + iterations
                });
                await LaserConnectionLogic.SendMessage(new LaserMessage
                {
                    RedLaser = 0,
                    BlueLaser = 0,
                    GreenLaser = 7,
                    X = 50 + iterations,
                    Y = 0
                });
                await LaserConnectionLogic.SendMessage(new LaserMessage
                {
                    RedLaser = 0,
                    BlueLaser = 7,
                    GreenLaser = 7,
                    X = 0,
                    Y = -50 - iterations,
                });
            }

            stopwatch.Stop();
            return $"{iterations} iterations";
        }
    }
}
