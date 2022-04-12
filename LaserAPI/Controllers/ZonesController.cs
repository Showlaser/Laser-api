using LaserAPI.Logic;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.FromFrontend.Zones;
using LaserAPI.Models.Helper;
using LaserAPI.Models.ToFrontend.Zones;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Controllers
{
    [Route("zone")]
    [ApiController]
    public class ZonesController : ControllerBase
    {
        private readonly ZoneLogic _zoneLogic;
        private readonly ControllerResultHandler _controllerResultHandler;

        public ZonesController(ZoneLogic zonesLogic, ControllerResultHandler controllerResultHandler)
        {
            _zoneLogic = zonesLogic;
            _controllerResultHandler = controllerResultHandler;
        }

        [HttpPost]
        public async Task<ActionResult> AddOrUpdate([FromBody] Zone zone)
        {
            async Task Action()
            {
                ZoneDto zoneDto = zone.Adapt<ZoneDto>();
                await _zoneLogic.AddOrUpdate(zoneDto);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpPost("play")]
        public async Task<ActionResult> PlayAnimation([FromBody] Zone zone)
        {
            async Task Action()
            {
                ZoneDto zoneDto = zone.Adapt<ZoneDto>();
                await _zoneLogic.Play(zoneDto);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpGet]
        public async Task<ActionResult<List<ZoneViewmodel>>> All()
        {
            async Task<List<ZoneViewmodel>> Action()
            {
                List<ZoneDto> zones = await _zoneLogic.All();
                return zones.Adapt<List<ZoneViewmodel>>();
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> Remove(Guid uuid)
        {
            async Task Action()
            {
                await _zoneLogic.Remove(uuid);
            }

            return await _controllerResultHandler.Execute(Action());
        }
    }
}