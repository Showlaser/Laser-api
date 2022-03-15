using LaserAPI.Models.Dto.Animations;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Models.Helper
{
    public static class PatternSettingsHelper
    {
        public static PatternAnimationSettingsDto GetSettingClosestToTimeMs(
            List<PatternAnimationSettingsDto> settings, int offsetTime, long timeMs)
        {
            List<PatternAnimationSettingsDto> settingsUnderTimeMs = settings.FindAll(s => s.StartTime + offsetTime < timeMs);
            return settingsUnderTimeMs.OrderByDescending(s => s.StartTime).FirstOrDefault();
        }
    }
}
