using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using LaserAPI.Models;
using Newtonsoft.Json;

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
                string json = JsonConvert.SerializeObject(new LaserMessage
                {
                    Rgbxy = new short[] {511, 255, 0, -4000, 2000 }
                }, Formatting.None);

                _laserConnection.SendMessage(json);
            }

            stopwatch.Stop();
            return $"{iterations} messages in {durationInSeconds} seconds";
        }

        [HttpPost]
        public void Post(int value)
        {
            for (int i = 0; i < 150000; i++)
            {
                string json = JsonConvert.SerializeObject(new LaserMessage
                {
                    Rgbxy = new short[] { 511, 255, 0, -4000, 2000 }
                }, Formatting.None);
                    
                _laserConnection.SendMessage(json);
            }
        }
    }
}
