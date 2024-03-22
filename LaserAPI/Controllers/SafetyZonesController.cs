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
    public class SafetyZonesController(ZoneLogic zonesLogic, ControllerResultHandler controllerResultHandler) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Save([FromBody] List<SafetyZone> zones)
        {
            async Task Action()
            {
                List<SafetyZoneDto> zoneDto = zones.Adapt<List<SafetyZoneDto>>();
                await zonesLogic.AddOrUpdate(zoneDto);
            }

            return await controllerResultHandler.Execute(Action());
        }

        [HttpPost("display")]
        public async Task<ActionResult> DisplaySafetyZone([FromBody] SafetyZone zone)
        {
            async Task Action()
            {
                SafetyZoneDto zoneDto = zone.Adapt<SafetyZoneDto>();
                await zonesLogic.Display(zoneDto);
            }

            return await controllerResultHandler.Execute(Action());
        }

        [HttpGet]
        public async Task<ActionResult<List<SafetyZoneViewmodel>>> All()
        {
            async Task<List<SafetyZoneViewmodel>> Action()
            {
                List<SafetyZoneDto> zones = await zonesLogic.All();
                return zones.Adapt<List<SafetyZoneViewmodel>>();
            }

            return await controllerResultHandler.Execute(Action());
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> Remove(Guid uuid)
        {
            async Task Action()
            {
                await zonesLogic.Remove(uuid);
            }

            return await controllerResultHandler.Execute(Action());
        }
    }
}