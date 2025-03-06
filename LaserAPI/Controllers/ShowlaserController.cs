using LaserAPI.Logic;
using LaserAPI.Models.Dto.RegisteredLaser;
using LaserAPI.Models.FromLaser;
using LaserAPI.Models.Helper;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LaserAPI.Controllers
{
    [Route("showlaser")]
    [ApiController]
    public class ShowlaserController(ControllerResultHandler _controllerResultHandler, LaserConnectionLogic _laserConnectionLogic) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<RegisteredLaserViewModel>> All()
        {
            static List<RegisteredLaserViewModel> Action()
            {
                List<RegisteredLaserDto> lasers = LaserConnectionLogicState.ConnectedLasers;
                return lasers.Adapt<List<RegisteredLaserViewModel>>();
            }

            return _controllerResultHandler.Execute(Action);
        }
    }
}
