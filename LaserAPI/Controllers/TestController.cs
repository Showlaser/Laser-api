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
            const int durationMs = 2000;
            const int max = 8;
            int iterations = 0;

            Stopwatch stopwatch = Stopwatch.StartNew();
            bool reverse = false;
            while (stopwatch.ElapsedMilliseconds < durationMs)
            {
                if (iterations < 1500 && !reverse)
                {
                    iterations += 5;
                }
                else if (iterations > 0 && reverse)
                {
                    iterations -= 5;
                }

                if (iterations == 1500)
                {
                    reverse = true;
                }

                if (iterations == 0)
                {
                    reverse = false;
                }

                await LaserConnectionLogic.SendMessage(new LaserMessage
                {
                    RedLaser = max,
                    BlueLaser = 0,
                    GreenLaser = 0,
                    X = -50 - iterations,
                    Y = 2000
                });
                await LaserConnectionLogic.SendMessage(new LaserMessage
                {
                    RedLaser = 0,
                    BlueLaser = max,
                    GreenLaser = 0,
                    X = 0,
                    Y = 2000 - iterations
                });
                await LaserConnectionLogic.SendMessage(new LaserMessage
                {
                    RedLaser = 0,
                    BlueLaser = 0,
                    GreenLaser = max,
                    X = 50 + iterations,
                    Y = 2000
                });
            }

            stopwatch.Stop();
            return $"{iterations} iterations";
        }
    }
}
