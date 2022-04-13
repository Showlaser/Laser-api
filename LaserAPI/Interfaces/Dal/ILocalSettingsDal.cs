using LaserAPI.Models.Dto;
using System.Threading.Tasks;

namespace LaserAPI.Interfaces.Dal
{
    public interface ILocalSettingsDal
    {
        Task Add(LocalSettingsDto localSettings);
        Task<LocalSettingsDto> Get();
        Task Update(LocalSettingsDto localSettings);
        Task<bool> Exists();
    }
}
