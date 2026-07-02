using LaserAPI.Logic;
using LaserAPI.Migrations;
using LaserAPI.Models.Dto.Patterns;
using LaserAPI.Models.Dto.RegisteredLaser;
using LaserAPI.Models.FromFrontend.Points;
using LaserAPI.Models.FromFrontend.Showlaser;
using LaserAPI.Models.FromFrontend.Showlaser.SDCard;
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
        public async Task<ActionResult<List<SDCardJsonFileWrapper>>> SDCardGetFiles(Guid uuid)
        {
            async Task<List<SDCardJsonFileWrapper>> Action()
            {

                RegisteredLaserDto registeredLaser = LaserConnectionLogicState.RegisteredLasers.Single(rl => rl.Uuid == uuid);
                return await _laserConnectionLogic.GetSDCardFiles(registeredLaser);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpPost("sd-card")]
        public async Task<ActionResult> SaveFile([FromBody] SDCardJsonFileWrapper fileWrapper)
        {
            async Task Action()
            {

                RegisteredLaserDto registeredLaser = LaserConnectionLogicState.RegisteredLasers.Single(rl => rl.Uuid == fileWrapper.RegisteredLaser.Uuid);
                await _laserConnectionLogic.SaveSDCardFile(fileWrapper);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpPost("play-lasershow")]
        public async Task<ActionResult> PlayLasershow([FromBody] SDCardJsonFileWrapper fileWrapper)
        {
            async Task Action()
            {
                _ = LaserConnectionLogicState.RegisteredLasers.Single(rl => rl.Uuid == fileWrapper.RegisteredLaser.Uuid);
                await _laserConnectionLogic.PlayLasershow(fileWrapper);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpPost("play-live")]
        public async Task<ActionResult> PlayLive([FromBody] SDCardJsonFileWrapper fileWrapper)
        {
            async Task Action()
            {
                _ = LaserConnectionLogicState.RegisteredLasers.Single(rl => rl.Uuid == fileWrapper.RegisteredLaser.Uuid);
                await _laserConnectionLogic.PlayLiveShow(fileWrapper);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpPost("stop")]
        public async Task<ActionResult> Stop([FromBody] RegisteredLaserDto registeredLaser)
        {
            async Task Action()
            {
                RegisteredLaserDto laser = LaserConnectionLogicState.RegisteredLasers.Single(rl => rl.Uuid == registeredLaser.Uuid);
                await _laserConnectionLogic.StopPlayback(laser);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpDelete("sd-card")]
        public async Task<ActionResult> SDCardDeleteFile(Guid uuid, string filename)
        {
            async Task Action()
            {

                RegisteredLaserDto registeredLaser = LaserConnectionLogicState.RegisteredLasers.Single(rl => rl.Uuid == uuid);
                SDCardJsonFileWrapper file = new()
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

        [HttpPost("safety-zone")]
        public async Task<ActionResult> ProjectSafetyZone([FromBody] ProjectSafetyZoneWrapper projectSafetyZoneWrapper)
        {
            async Task Action()
            {
                await _laserConnectionLogic.ProjectSafetyZone(projectSafetyZoneWrapper);
            }

            return await _controllerResultHandler.Execute(Action());
        }
    }
}
