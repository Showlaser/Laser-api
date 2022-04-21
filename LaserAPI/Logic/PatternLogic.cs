using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Patterns;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

        public void PlayPattern(PatternDto pattern)
        {
            ValidatePattern(pattern);
            Stopwatch stopwatch = Stopwatch.StartNew();
            pattern.Points = pattern.Points.OrderBy(p => p.Order).ToList();

            while (stopwatch.ElapsedMilliseconds < 500)
            {
                List<LaserMessage> messages = new();

                int pointsLength = pattern.Points.Count;
                for (int index = 0; index < pointsLength; index++)
                {
                    PointDto point = pattern.Points[index];
                    messages.Add(new LaserMessage
                    {
                        RedLaser = point.RedLaserPowerPwm,
                        GreenLaser = point.GreenLaserPowerPwm,
                        BlueLaser = point.BlueLaserPowerPwm,
                        X = point.X,
                        Y = point.Y,
                    });
                }

                _laserLogic.SendData(messages);
                messages.Clear();
            }

            stopwatch.Stop();
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
