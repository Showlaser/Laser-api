using LaserAPI.Models.Dto;
using LaserAPI.Models.FromFrontend;
using LaserAPI.Models.Helper;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LaserAPI.Controllers
{
    [Route("settings")]
    [ApiController]
    public class LocalSettingsController : ControllerBase
    {
        private readonly ControllerResultHandler _controllerResultHandler;

        public LocalSettingsController(ControllerResultHandler controllerResultHandler)
        {
            _controllerResultHandler = controllerResultHandler;
        }

        [HttpPost]
        public async Task<ActionResult> SetSettings([FromBody] LocalSettings localSettings)
        {
            async Task Action()
            {
                LocalSettingsDto localSettingsDto = localSettings.Adapt<LocalSettingsDto>();

            }

            return await _controllerResultHandler.Execute(Action());
        }
    }
}
