using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class ZoneLogic
    {
        private readonly IZoneDal _zoneDal;
        private static List<SafetyZoneDto> _zones;
        public static int ZonesLength { get; private set; }
        public static SafetyZoneDto ZoneWithLowestLaserPowerPwm { get; private set; }

        public ZoneLogic(IZoneDal zoneDal)
        {
            _zoneDal = zoneDal;
            //UpdateZones();
        }

        private void UpdateZones()
        {
            _zones = _zoneDal
                .All()
                .Result;

            _zones.ForEach(zone =>
            {
                zone.Points = zone.Points.OrderBy(p => p.OrderNr).ToList();
            });

            ProjectionZonesLogic.Zones = _zones;
            ZonesLength = _zones.Count;
            ZoneWithLowestLaserPowerPwm = _zones.MinBy(zone => zone.MaxLaserPowerInZonePercentage);
        }

        public async Task<List<SafetyZoneDto>> All()
        {
            return await _zoneDal.All();
        }

        private static void ValidateZone(SafetyZoneDto zone)
        {
            bool zonePointsValid = zone.Points.TrueForAll(p => p.X.IsBetweenOrEqualTo(-4000, 4000) &&
                                                               p.Y.IsBetweenOrEqualTo(-4000, 4000) &&
                                                               p.Uuid != Guid.Empty
                                                               && p.SafetyZoneUuid == zone.Uuid) && zone.Points.Count != 0;

            bool zoneValid = zone.Uuid != Guid.Empty &&
                !string.IsNullOrEmpty(zone.Name) &&
                zone.MaxLaserPowerInZonePercentage.IsBetweenOrEqualTo(0, 100) &&
                zonePointsValid;

            if (!zoneValid)
            {
                throw new InvalidOperationException();
            }
        }

        public async Task AddOrUpdate(List<SafetyZoneDto> zones)
        {
            foreach (SafetyZoneDto zone in zones)
            {
                ValidateZone(zone);
                if (await _zoneDal.Exists(zone.Uuid))
                {
                    await _zoneDal.Update(zone);
                    UpdateZones();
                }
                else
                {
                    await _zoneDal.Add(zone);
                    UpdateZones();
                }
            }
        }

        public async Task Display(SafetyZoneDto zone)
        {
            ValidateZone(zone);
            zone.Points = zone.Points.OrderBy(p => p.OrderNr).ToList();
            List<LaserMessage> messages = zone.Points
                .Select(p =>
                {
                    LaserMessage message = new(zone.MaxLaserPowerInZonePercentage, 0, 0, p.X, p.Y); // todo do something if power goes over 255 add other color
                    return message;
                })
                .ToList();
            messages.Add(messages.First());
            //await LaserConnectionLogic.SendMessages(new LaserCommando(1000, messages.ToArray()));
        }

        public async Task Remove(Guid uuid)
        {
            await _zoneDal.Remove(uuid);
            UpdateZones();
        }
    }
}
