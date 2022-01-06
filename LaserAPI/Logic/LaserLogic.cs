using LaserAPI.Interfaces.Dal;
using LaserAPI.Interfaces.Helper;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper.Laser;
using LaserAPI.Models.Helper.Zones;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Logic
{
    public class LaserLogic
    {
        private readonly ILaserConnectionLogic _laserSafetyLogic;
        private readonly List<ZoneDto> _zones;
        private readonly bool _skipZonesCheck;

        public LaserLogic(ILaserConnectionLogic laserSafetyLogic, IZoneDal zoneDal)
        {
            _laserSafetyLogic = laserSafetyLogic;
            _zones = zoneDal.All().Result;
            _skipZonesCheck = _zones.Any();
        }

        public void SendData(LaserMessage message)
        {
            if (_skipZonesCheck)
            {
                _laserSafetyLogic.SendMessage(message);
                return;
            }

            ZoneDto zone = ZonesHelper.GetZoneWherePositionIsIn(_zones, message.X, message.Y);
            bool positionIsInSafetyZone = zone != null;

            if (positionIsInSafetyZone)
            {
                LaserSafetyHelper.LimitLaserPowerIfNecessary(ref message, zone.MaxLaserPowerInZonePwm);
            }

            _laserSafetyLogic.SendMessage(message);
        }
    }
}
