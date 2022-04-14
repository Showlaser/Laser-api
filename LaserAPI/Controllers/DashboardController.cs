using LaserAPI.Enums;
using LaserAPI.Logic;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using LaserAPI.Models.ToFrontend.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Controllers
{
    [Route("dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ControllerResultHandler _controllerResultHandler;
        private readonly ZoneLogic _zoneLogic;
        private readonly AnimationLogic _animationLogic;

        public DashboardController(ControllerResultHandler controllerResultHandler, ZoneLogic zoneLogic, AnimationLogic animationLogic)
        {
            _controllerResultHandler = controllerResultHandler;
            _zoneLogic = zoneLogic;
            _animationLogic = animationLogic;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardViewmodel>> GetDashboardData()
        {
            async Task<DashboardViewmodel> Action()
            {
                List<ZoneDto> zones = await _zoneLogic.All();
                int zonesLength = zones.Count;
                bool developmentModeActive = zones.Any(z => z.MaxLaserPowerInZonePwm == 20 && z.Points.Count == 4) &&
                                             zonesLength == 1;

                List<AnimationDto> animations = await _animationLogic.All();
                return new DashboardViewmodel
                {
                    LaserSettings = new LaserSettingsViewmodel(zones.Count, developmentModeActive),
                    ApplicationStatus = new ApplicationStatusViewmodel(LaserConnectionLogic.Client?.Connected is true, LaserConnectionLogic.ComputerIpAddress),
                    Logs = GetLogMessages(developmentModeActive, zonesLength),
                    Shows = animations.Select(a => new ShowViewmodel(a.Name)).ToList()
                };
            }

            return await _controllerResultHandler.Execute(Action());
        }

        private static List<LogViewmodel> GetLogMessages(bool developmentModeActive, int zonesLength)
        {
            List<LogViewmodel> logs = new();
            LogViewmodel developmentModeLog = developmentModeActive
                ? new LogViewmodel("Development mode active laser power is limited", LogType.Info)
                : new LogViewmodel("Development mode is not active, watch out for high powered beams!",
                    LogType.Warning);

            logs.Add(developmentModeLog);
            if (zonesLength == 0)
            {
                logs.Add(new LogViewmodel("No projection zones found, the laser will not output power!", LogType.Error));
            }

            return logs;
        }
    }
}
