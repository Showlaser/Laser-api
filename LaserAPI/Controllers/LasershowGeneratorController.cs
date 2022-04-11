using LaserAPI.Logic;
using LaserAPI.Models.FromFrontend.LasershowGenerator;
using LaserAPI.Models.Helper;
using Microsoft.AspNetCore.Mvc;
using NAudio.CoreAudioApi;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Controllers
{
    [Route("lasershow-generator")]
    [ApiController]
    public class LasershowGeneratorController : ControllerBase
    {
        private readonly LaserShowGeneratorLogic _laserShowGeneratorLogic;

        public LasershowGeneratorController(LaserShowGeneratorLogic laserShowGeneratorLogic)
        {
            _laserShowGeneratorLogic = laserShowGeneratorLogic;
        }

        [HttpGet("devices")]
        public ActionResult<IEnumerable<string>> GetDevices()
        {
            IEnumerable<string> Action()
            {
                MMDeviceCollection devices = _laserShowGeneratorLogic.GetDevices();
                return devices.Select(d => d.FriendlyName);
            }

            ControllerResultHandler controllerResultHandler = new();
            return controllerResultHandler.Execute(Action);
        }

        [HttpPost("start")]
        public void Start([FromBody] SongData songData, [FromQuery] string deviceName)
        {
            void Action()
            {
                _laserShowGeneratorLogic.SetSongData(songData);
                _laserShowGeneratorLogic.Start(deviceName);
            }

            ControllerResultHandler controllerResultHandler = new();
            controllerResultHandler.Execute(Action);
        }

        [HttpPost("stop")]
        public void Stop()
        {
            ControllerResultHandler controllerResultHandler = new();
            controllerResultHandler.Execute(_laserShowGeneratorLogic.Stop);
        }

        [HttpPost("data")]
        public void SetSongData([FromBody] SongData songData)
        {
            ControllerResultHandler controllerResultHandler = new();
            controllerResultHandler.Execute(() => _laserShowGeneratorLogic.SetSongData(songData));
        }
    }
}
