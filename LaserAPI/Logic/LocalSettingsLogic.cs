using LaserAPI.Models.Dto;
using System.IO;

namespace LaserAPI.Logic
{
    public class LocalSettingsLogic
    {
        private static void ValidateSettings(LocalSettingsDto localSettings)
        {
            bool settingsInvalid = string.IsNullOrEmpty(localSettings.IpAddress);
            if (settingsInvalid)
            {
                throw new InvalidDataException();
            }
        }

        public void AddOrUpdate(LocalSettingsDto localSettings)
        {
            ValidateSettings(localSettings);
        }
    }
}
