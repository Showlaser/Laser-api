using LaserAPI.Logic;
using LaserAPI.Models.Dto.Lasershow;
using LaserAPI.Models.FromFrontend.Lasershow;
using LaserAPI.Models.Helper;
using LaserAPI.Models.ToFrontend.Lasershow;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Controllers
{
    [Route("lasershow")]
    [ApiController]
    public class LasershowController : ControllerBase
    {
        private readonly LasershowLogic _lasershowLogic;

        public LasershowController(LasershowLogic lasershowLogic)
        {
            _lasershowLogic = lasershowLogic;
        }

        [HttpPost]
        public async Task<ActionResult> AddOrUpdate([FromBody] Lasershow animation)
        {
            async Task Action()
            {
                LasershowDto lasershowDto = animation.Adapt<LasershowDto>();
                await _lasershowLogic.AddOrUpdate(lasershowDto);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }

        [HttpPost("play")]
        public async Task<ActionResult> PlayLasershow([FromBody] Lasershow lasershow)
        {
            async Task Action()
            {
                LasershowDto lasershowDto = lasershow.Adapt<LasershowDto>();
                await _lasershowLogic.PlayLasershow(lasershowDto);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }

        [HttpGet]
        public async Task<ActionResult<List<LasershowViewmodel>>> All()
        {
            async Task<List<LasershowViewmodel>> Action()
            {
                List<LasershowDto> lasershows = await _lasershowLogic.All();
                return lasershows.Adapt<List<LasershowViewmodel>>();
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> Remove(Guid uuid)
        {
            async Task Action()
            {
                await _lasershowLogic.Remove(uuid);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }
    }
}
