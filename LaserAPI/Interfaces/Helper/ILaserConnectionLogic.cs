using LaserAPI.Models.Helper.Laser;
using System.Threading.Tasks;

namespace LaserAPI.Interfaces.Helper
{
    public interface ILaserConnectionLogic
    {
        Task Connect();
        void Disconnect();
        void SendMessage(LaserMessage message);
    }
}
