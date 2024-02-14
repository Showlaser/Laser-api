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
        private static List<ZoneDto> _zones;
        public static int ZonesLength { get; private set; }
        public static ZoneDto ZoneWithLowestLaserPowerPwm { get; private set; }

        public ZoneLogic(IZoneDal zoneDal)
        {
            _zoneDal = zoneDal;
            UpdateZones();
        }

        private void UpdateZones()
        {
            _zones = _zoneDal
                .All()
                .Result;

            _zones.ForEach(zone =>
            {
                zone.Points = zone.Points.OrderBy(p => p.Order).ToList();
            });

            ProjectionZonesLogic.Zones = _zones;
            ZonesLength = _zones.Count;
            ZoneWithLowestLaserPowerPwm = _zones.MinBy(zone => zone.MaxLaserPowerInZonePwm);
        }

        public async Task<List<ZoneDto>> All()
        {
            return await _zoneDal.All();
        }

        private static void ValidateZone(ZoneDto zone)
        {
            bool zonePointsValid = zone.Points.TrueForAll(p => p.X.IsBetweenOrEqualTo(-4000, 4000) &&
                                                               p.Y.IsBetweenOrEqualTo(-4000, 4000) &&
                                                               p.Uuid != Guid.Empty
                                                               && p.ZoneUuid == zone.Uuid) && zone.Points.Any();

            bool zoneValid = zone.Uuid != Guid.Empty && !string.IsNullOrEmpty(zone.Name) &&
                zone.MaxLaserPowerInZonePwm.IsBetweenOrEqualTo(0, 765) && zonePointsValid;

            if (!zoneValid)
            {
                throw new InvalidOperationException();
            }
        }

        public async Task AddOrUpdate(ZoneDto zone)
        {
            ValidateZone(zone);
            if (await _zoneDal.Exists(zone.Uuid))
            {
                await _zoneDal.Update(zone);
                UpdateZones();
                return;
            }

            await _zoneDal.Add(zone);
            UpdateZones();
        }

        public async Task Play(ZoneDto zone)
        {
            ValidateZone(zone);
            zone.Points = zone.Points.OrderBy(p => p.Order).ToList();
            List<LaserMessage> messages = zone.Points
                .Select(p =>
                {
                    LaserMessage message = new(zone.MaxLaserPowerInZonePwm, 0, 0, p.X, p.Y); // todo do something if power goes over 255 add other color
                    return message;
                })
                .ToList();
            messages.Add(messages.First());
            await LaserConnectionLogic.SendMessages(new LaserCommando(1000, messages.ToArray()));
        }

        public async Task Remove(Guid uuid)
        {
            await _zoneDal.Remove(uuid);
            UpdateZones();
        }
    }
}
