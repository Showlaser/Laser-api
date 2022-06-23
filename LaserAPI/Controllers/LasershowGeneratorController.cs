using LaserAPI.Logic;
using LaserAPI.Models.FromFrontend.LasershowGenerator;
using LaserAPI.Models.Helper;
using LaserAPI.Models.ToFrontend.LasershowGenerator;
using Microsoft.AspNetCore.Mvc;
using NAudio.CoreAudioApi;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LaserAPI.Controllers
{
    [Route("lasershow-generator")]
    [ApiController]
    public class LasershowGeneratorController : ControllerBase
    {
        private readonly ControllerResultHandler _controllerResultHandler;
        private readonly LasershowGeneratorConnectedSongSelector _lasershowGeneratorConnectedSongSelector;

        public LasershowGeneratorController(ControllerResultHandler controllerResultHandler,
            LasershowGeneratorConnectedSongSelector lasershowGeneratorConnectedSongSelector)
        {
            _controllerResultHandler = controllerResultHandler;
            _lasershowGeneratorConnectedSongSelector = lasershowGeneratorConnectedSongSelector;
        }

        [HttpGet("devices")]
        public ActionResult<IEnumerable<string>> GetDevices()
        {
            static IEnumerable<string> Action()
            {
                MMDeviceCollection devices = LaserShowGeneratorAlgorithm.GetDevices();
                return devices.Select(d => d.FriendlyName);
            }

            return _controllerResultHandler.Execute(Action);
        }
        
        [HttpGet("status")]
        public ActionResult<LaserGeneratorStatusViewmodel> GetStatus()
        {
            return _controllerResultHandler.Execute(() => LaserShowGeneratorAlgorithm.GetStatus);
        }

        [HttpPost("start")]
        public void Start([FromBody] SongData songData, [FromQuery] string deviceName)
        {
            _controllerResultHandler.Execute(() => _lasershowGeneratorConnectedSongSelector.Start(songData, deviceName));
        }

        [HttpPost("stop")]
        public void Stop()
        {
            _controllerResultHandler.Execute(_lasershowGeneratorConnectedSongSelector.Stop);
        }

        [HttpPost("data")]
        public void SetSongData([FromBody] SongData songData)
        {
            _controllerResultHandler.Execute(() => LaserShowGeneratorAlgorithm.SetSongData(songData));
        }
    }
}
