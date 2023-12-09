using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Patterns;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class PatternLogic
    {
        private readonly IPatternDal _patternDal;
        private readonly LaserLogic _laserLogic;

        public PatternLogic(IPatternDal patternDal, LaserLogic laserLogic)
        {
            _patternDal = patternDal;
            _laserLogic = laserLogic;
        }

        private static bool PointsAreValid(PointDto points)
        {
            return points.Uuid != Guid.Empty &&
                points.PatternUuid != Guid.Empty &&
                points.RedLaserPowerPwm.IsBetweenOrEqualTo(0, 255) &&
                points.GreenLaserPowerPwm.IsBetweenOrEqualTo(0, 255) &&
                points.BlueLaserPowerPwm.IsBetweenOrEqualTo(0, 255) &&
                points.OrderNr >= 0 &&
                points.X.IsBetweenOrEqualTo(-4000, 4000) &&
                points.Y.IsBetweenOrEqualTo(-4000, 4000);
        }

        public static bool PatternIsValid(PatternDto pattern)
        {
            return pattern.Uuid != Guid.Empty &&
                !string.IsNullOrEmpty(pattern.Name) &&
                !string.IsNullOrEmpty(pattern.Image) &&
                pattern.Points.TrueForAll(PointsAreValid) &&
                pattern.Scale.IsBetweenOrEqualTo(0.1, 10) &&
                pattern.XOffset.IsBetweenOrEqualTo(-4000, 4000) &&
                pattern.YOffset.IsBetweenOrEqualTo(-4000, 4000) &&
                pattern.Rotation.IsBetweenOrEqualTo(-360, 360);
        }

        public async Task AddOrUpdate(PatternDto pattern)
        {
            if (await _patternDal.Exists(pattern.Uuid))
            {
                await _patternDal.Update(pattern);
                return;
            }

            await _patternDal.Add(pattern);
        }

        public async Task PlayPattern(PatternDto pattern)
        {
            pattern.Points = pattern.Points.OrderBy(p => p.OrderNr).ToList();
            IReadOnlyList<LaserMessage> messages = pattern.Points.Select(p =>
                new LaserMessage(p.RedLaserPowerPwm, p.GreenLaserPowerPwm, p.BlueLaserPowerPwm, p.X, p.Y))
                .ToList();

            await LaserLogic.SendData(messages, 1000);
        }

        public async Task<List<PatternDto>> All()
        {
            return await _patternDal.All();
        }

        public async Task Update(PatternDto pattern)
        {
            await _patternDal.Update(pattern);
        }

        public async Task Remove(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                throw new NoNullAllowedException(nameof(uuid));
            }
            await _patternDal.Remove(uuid);
        }
    }
}
