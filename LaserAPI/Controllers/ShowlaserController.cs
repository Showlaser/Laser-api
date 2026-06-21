using LaserAPI.Logic;
using LaserAPI.Models.Dto.RegisteredLaser;
using LaserAPI.Models.FromFrontend.Points;
using LaserAPI.Models.FromLaser;
using LaserAPI.Models.Helper;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                List<RegisteredLaserDto> lasers = LaserConnectionLogicState.RegisteredLasers;
                return lasers.Adapt<List<RegisteredLaserViewModel>>();
            }

            return _controllerResultHandler.Execute(Action);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] RegisteredLaserDto registeredLaser)
        {
            async Task Action()
            {
                await _laserConnectionLogic.UpdateShowlaser(registeredLaser);
            }

            return await _controllerResultHandler.Execute(Action());
        }


        [HttpDelete("{uuid}")]
        public async Task<ActionResult> RemoveLaser(Guid uuid)
        {
            async Task Action()
            {
                await _laserConnectionLogic.Remove(uuid);
            }

            return await _controllerResultHandler.Execute(Action());
        }
    }
}
