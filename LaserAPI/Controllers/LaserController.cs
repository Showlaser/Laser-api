using LaserAPI.Logic;
using LaserAPI.Models.FromFrontend.Points;
using LaserAPI.Models.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Controllers
{
    [Route("laser")]
    [ApiController]
    public class LaserController(LaserLogic _laserLogic, ControllerResultHandler _controllerResultHandler) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> ProjectPoints([FromBody] List<PointWrapper> points)
        {
            async Task Action()
            {
                await _laserLogic.RenderProvidedPoints(points);
            }

            return await _controllerResultHandler.Execute(Action());
        }
    }
}
