using LaserAPI.Logic;
using LaserAPI.Migrations;
using LaserAPI.Models.Dto.RegisteredLaser;
using LaserAPI.Models.FromFrontend.Points;
using LaserAPI.Models.FromLaser;
using LaserAPI.Models.Helper;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
                return LaserConnectionLogicState.RegisteredLasers.Adapt<List<RegisteredLaserViewModel>>();
            }

            return _controllerResultHandler.Execute(Action);
        }

        [HttpGet("sd-card")]
        public async Task<ActionResult<List<SDCardJsonFile>>> SDCardGetFiles(Guid uuid)
        {
            async Task<List<SDCardJsonFile>> Action()
            {

                RegisteredLaserDto registeredLaser = LaserConnectionLogicState.RegisteredLasers.Single(rl => rl.Uuid == uuid);
                return await _laserConnectionLogic.GetSDCardFiles(registeredLaser);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpDelete("sd-card")]
        public async Task<ActionResult> SDCardDeleteFile(Guid uuid, string filename)
        {
            async Task Action()
            {

                RegisteredLaserDto registeredLaser = LaserConnectionLogicState.RegisteredLasers.Single(rl => rl.Uuid == uuid);
                SDCardJsonFile file = new()
                {
                    Filename = filename
                };
                await _laserConnectionLogic.DeleteSDCardFile(registeredLaser, file);
            }

            return await _controllerResultHandler.Execute(Action());
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
