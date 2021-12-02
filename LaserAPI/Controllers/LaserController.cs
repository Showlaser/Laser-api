using LaserAPI.Models;
using Microsoft.AspNetCore.Mvc;
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
        public void SendMessage()
        {
            string json = JsonConvert.SerializeObject(new LaserMessage
            {
                Rgbxy = new short[] { 511, 511, 511, -4000, 2000 }
            }, Formatting.None);

            _laserConnection.SendMessage(json);
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
