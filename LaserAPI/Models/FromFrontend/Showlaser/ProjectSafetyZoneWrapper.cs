using LaserAPI.Models.Dto.Patterns;
using LaserAPI.Models.Dto.RegisteredLaser;

namespace LaserAPI.Models.FromFrontend.Showlaser
{
    public class ProjectSafetyZoneWrapper
    {
        public RegisteredLaserDto RegisteredLaser { get; set; }
        public PatternDto SafetyZone { get; set; }
    }
}
