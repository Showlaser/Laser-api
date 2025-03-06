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
    [Route("laserconnection")]
    [ApiController]
    public class LaserConnectionController(LaserConnectionLogic _laserConnectionLogic, ControllerResultHandler _controllerResultHandler) : ControllerBase
    {
        [HttpPost("connect")]
        public async Task<ActionResult> Connect([FromBody] RegisteredLaser registeredLaser)
        {
            async Task Action()
            {
                RegisteredLaserDto registeredLaserDto = registeredLaser.Adapt<RegisteredLaserDto>();
                await _laserConnectionLogic.Connect(registeredLaserDto);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpGet]
        public async Task<ActionResult<List<RegisteredLaserDto>>> GetStatuses()
        {
            static async Task<List<RegisteredLaserDto>> Action()
            {
                return await LaserConnectionLogic.GetStatus();
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpPost]
        public async Task<ActionResult> ProjectPoints([FromBody] List<PointWrapper> points)
        {
            async Task Action()
            {
                await LaserConnectionLogic.SendData(points);
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
