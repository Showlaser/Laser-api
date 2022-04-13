using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto;
using System.IO;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LocalSettingsLogic
    {
        private readonly ILocalSettingsDal _localSettingsDal;

        public LocalSettingsLogic(ILocalSettingsDal localSettingsDal)
        {
            _localSettingsDal = localSettingsDal;
        }

        private static void ValidateSettings(LocalSettingsDto localSettings)
        {
            bool settingsInvalid = string.IsNullOrEmpty(localSettings.IpAddress);
            if (settingsInvalid)
            {
                throw new InvalidDataException();
            }
        }

        public async Task AddOrUpdate(LocalSettingsDto localSettings)
        {
            ValidateSettings(localSettings);
        }
    }
}
