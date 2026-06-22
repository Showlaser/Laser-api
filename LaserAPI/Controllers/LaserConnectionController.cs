using LaserAPI.Interfaces;
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
    public class LaserConnectionController(ILaserConnectionLogic _laserConnectionLogic, ControllerResultHandler _controllerResultHandler) : ControllerBase
    {
        [HttpPost("connect")]
        public async Task<ActionResult> Connect([FromBody] RegisteredLaser registeredLaser)
        {
            async Task Action()
            {
                Console.WriteLine($"[{DateTime.Now}] Incoming connection request from showlaser: {registeredLaser.Name}");
                RegisteredLaserDto registeredLaserDto = registeredLaser.Adapt<RegisteredLaserDto>();
                await _laserConnectionLogic.Connect(registeredLaserDto);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpPut()]
        public async Task<ActionResult> Update([FromBody] RegisteredLaser registeredLaser)
        {
            async Task Action()
            {
                Console.WriteLine($"[{DateTime.Now}] Incoming update request from showlaser: {registeredLaser.Name}");
                RegisteredLaserDto registeredLaserDto = registeredLaser.Adapt<RegisteredLaserDto>();
                await _laserConnectionLogic.Update(registeredLaserDto);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpGet("alive")]
        public ActionResult AliveCheck(Guid uuid)
        {
            void Action()
            {
                if (!LaserConnectionLogicState.RegisteredLasers.Exists(rl => rl.Uuid == uuid))
                {
                    throw new KeyNotFoundException();
                }
            }

            return _controllerResultHandler.Execute(Action);
        }
    }
}
