using LaserAPI.Logic;
using LaserAPI.Models.FromFrontend.LasershowGenerator;
using LaserAPI.Models.Helper;
using LaserAPI.Models.ToFrontend;
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
        private readonly ControllerResultHandler _controllerResultHandler;

        public LasershowGeneratorController(LaserShowGeneratorLogic laserShowGeneratorLogic, ControllerResultHandler controllerResultHandler)
        {
            _laserShowGeneratorLogic = laserShowGeneratorLogic;
            _controllerResultHandler = controllerResultHandler;
        }

        [HttpGet("devices")]
        public ActionResult<IEnumerable<string>> GetDevices()
        {
            IEnumerable<string> Action()
            {
                MMDeviceCollection devices = _laserShowGeneratorLogic.GetDevices();
                return devices.Select(d => d.FriendlyName);
            }

            return _controllerResultHandler.Execute(Action);
        }

        [HttpGet("status")]
        public ActionResult<LaserGeneratorStatusViewmodel> GetStatus()
        {
            return _controllerResultHandler.Execute(() => _laserShowGeneratorLogic.GetStatus);
        }

        [HttpPost("start")]
        public void Start([FromBody] SongData songData, [FromQuery] string deviceName)
        {
            void Action()
            {
                _laserShowGeneratorLogic.SetSongData(songData);
                _laserShowGeneratorLogic.Start(deviceName);
            }

            _controllerResultHandler.Execute(Action);
        }

        [HttpPost("stop")]
        public void Stop()
        {
            _controllerResultHandler.Execute(_laserShowGeneratorLogic.Stop);
        }

        [HttpPost("data")]
        public void SetSongData([FromBody] SongData songData)
        {
            _controllerResultHandler.Execute(() => _laserShowGeneratorLogic.SetSongData(songData));
        }
    }
}
