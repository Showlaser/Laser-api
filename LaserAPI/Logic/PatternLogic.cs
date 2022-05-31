using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Patterns;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

        private static bool ValidatePoints(List<PointDto> points)
        {
            return points.Any() && points.TrueForAll(p => p.PatternUuid != Guid.Empty &&
                                                          p.Y.IsBetweenOrEqualTo(-4000, 4000) &&
                                                          p.X.IsBetweenOrEqualTo(-4000, 4000) &&
                                                          p.RedLaserPowerPwm.IsBetweenOrEqualTo(0, 255) &&
                                                          p.GreenLaserPowerPwm.IsBetweenOrEqualTo(0, 255) &&
                                                          p.BlueLaserPowerPwm.IsBetweenOrEqualTo(0, 255));
        }

        private static void ValidatePattern(PatternDto pattern)
        {
            bool patternValid = pattern != null && pattern.Scale.IsBetweenOrEqualTo(0.1, 1) && ValidatePoints(pattern.Points);
            if (!patternValid)
            {
                throw new InvalidDataException(nameof(pattern));
            };
        }

        public async Task AddOrUpdate(PatternDto pattern)
        {
            ValidatePattern(pattern);
            if (await _patternDal.Exists(pattern.Uuid))
            {
                await _patternDal.Update(pattern);
                return;
            }

            await _patternDal.Add(pattern);
        }

        public async Task PlayPattern(PatternDto pattern)
        {
            ValidatePattern(pattern);
            pattern.Points = pattern.Points.OrderBy(p => p.Order).ToList();
            IReadOnlyList<LaserMessage> messages = pattern.Points.Select(p =>
                new LaserMessage(p.RedLaserPowerPwm, p.GreenLaserPowerPwm, p.BlueLaserPowerPwm, p.X, p.Y))
                .ToList();

            await _laserLogic.SendData(messages, 1000);
        }

        public async Task<List<PatternDto>> All()
        {
            return await _patternDal.All();
        }

        public async Task Update(PatternDto pattern)
        {
            ValidatePattern(pattern);
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
