using LaserAPI.Logic;
using LaserAPI.Models.Dto.Lasershows;
using LaserAPI.Models.FromFrontend.Lasershows;
using LaserAPI.Models.Helper;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Controllers
{
    [Route("lasershow")]
    [ApiController]
    public class LasershowController(LasershowLogic _lasershowLogic, ControllerResultHandler _controllerResultHandler) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate([FromBody] Lasershow lasershow)
        {
            async Task Action()
            {
                LasershowDto lasershowDto = lasershow.Adapt<LasershowDto>();
                await _lasershowLogic.AddOrUpdate(lasershowDto);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpGet]
        public async Task<ActionResult<List<LasershowDto>>> All()
        {
            async Task<List<LasershowDto>> Action()
            {
                return await _lasershowLogic.All();
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> Remove(Guid uuid)
        {
            async Task Action()
            {
                await _lasershowLogic.Remove(uuid);
            }

            return await _controllerResultHandler.Execute(Action());
        }
    }
}
