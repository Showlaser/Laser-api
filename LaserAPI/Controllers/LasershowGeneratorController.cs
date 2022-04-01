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

            ControllerErrorHandler controllerErrorHandler = new();
            return controllerErrorHandler.Execute(Action);
        }

        [HttpPost("start")]
        public void Start([FromBody] SongData songData, [FromQuery] string deviceName)
        {
            void Action()
            {
                _laserShowGeneratorLogic.SetSongData(songData);
                _laserShowGeneratorLogic.Start(deviceName);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            controllerErrorHandler.Execute(Action);
        }

        [HttpPost("stop")]
        public void Stop()
        {
            ControllerErrorHandler controllerErrorHandler = new();
            controllerErrorHandler.Execute(_laserShowGeneratorLogic.Stop);
        }

        [HttpPost("data")]
        public void SetSongData([FromBody] SongData songData)
        {
            ControllerErrorHandler controllerErrorHandler = new();
            controllerErrorHandler.Execute(() => _laserShowGeneratorLogic.SetSongData(songData));
        }
    }
}
