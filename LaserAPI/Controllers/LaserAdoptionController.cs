using LaserAPI.Interfaces;
using LaserAPI.Logic;
using LaserAPI.Models.Dto.Lasershows;
using LaserAPI.Models.Dto.RegisteredLaser;
using LaserAPI.Models.FromFrontend.Lasershows;
using LaserAPI.Models.FromLaser;
using LaserAPI.Models.Helper;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Controllers
{
    [Route("adoption")]
    [ApiController]
    public class LaserAdoptionController(ILaserConnectionLogic _laserConnectionLogic, ControllerResultHandler _controllerResultHandler) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<UDPBroadcast>> GetPendingAdoptions()
        {
            static List<UDPBroadcast> Action()
            {
                return LaserConnectionLogic.GetPendingAdoptions();
            }

            return _controllerResultHandler.Execute(Action);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Adopt([FromBody] RegisteredLaser registeredLaser)
        {
            async Task<bool> Action()
            {
                RegisteredLaserDto registeredLaserDto = registeredLaser.Adapt<RegisteredLaserDto>();
                return await _laserConnectionLogic.Adopt(registeredLaserDto);
            }

            return await _controllerResultHandler.Execute(Action());
        }
    }
}
