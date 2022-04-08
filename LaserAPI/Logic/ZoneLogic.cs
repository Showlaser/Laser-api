using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class ZoneLogic
    {
        private readonly IZoneDal _zoneDal;

        public ZoneLogic(IZoneDal zoneDal)
        {
            _zoneDal = zoneDal;
        }

        public async Task<List<ZoneDto>> All()
        {
            return await _zoneDal.All();
        }

        public async Task AddOrUpdate(ZoneDto zone)
        {
            if (zone.Positions.Count != 4)
            {
                throw new InvalidDataException();
            }
            if (await _zoneDal.Exists(zone.Uuid))
            {
                await _zoneDal.Update(zone);
                return;
            }

            await _zoneDal.Add(zone);
        }

        public async Task Remove(Guid uuid)
        {
            await _zoneDal.Remove(uuid);
        }

        /// <summary>
        /// Checks if the path of the previous position and the new position crosses a zoneHitData
        /// </summary>
        /// <param name="zone"></param>
        /// <returns>The zones that are crossed</returns>
        public static List<ZoneLine> GetLineHitByPath(ZoneDto zone, LaserMessage message)
        {
            List<ZoneLine> points = new();

            for (int i = 0; i < 4; i++)
            {
                ZonesPositionDto position = zone.Positions[i];
                Point zonePoint1 = new(position.X, position.Y);

                for (int j = 0; j < 4; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    ZonesPositionDto secondPosition = zone.Positions[j];
                    Point zonePoint2 = new(secondPosition.X, secondPosition.Y);

                    Point crossedPoint = CalculateCrossingPointOfTwoLines(message, zonePoint1, zonePoint2);
                    if (crossedPoint.X != -4001 && crossedPoint.Y != -4001)
                    {
                        points.Add(new ZoneLine
                        {
                            Point1 = zonePoint1,
                            Point2 = zonePoint2,
                            CrossedPoint = crossedPoint
                        });
                    }
                }
            }

            var list = points.DistinctBy(p => p.CrossedPoint).ToList();
            return list;
        }

        public static ZoneDto GetZoneWhereMaxLaserPowerPwmIsHighest(List<ZoneDto> zones)
        {
            return zones.MaxBy(zone => zone.MaxLaserPowerInZonePwm);
        }

        /// <summary>
        /// This method calculates the crossing x and y point with two lines
        /// </summary>
        /// <param name="newMessage">The new position</param>
        /// <param name="point1">The first point of the line</param>
        /// <param name="point2">The second point of the line</param>
        /// <returns>A new point with the x and y position of the crossing point if the lines do not cross a point with the values -4001, -4001 is returned</returns>
        public static Point CalculateCrossingPointOfTwoLines(LaserMessage newMessage, Point point1, Point point2)
        {
            int currentXPosition = LaserConnectionLogic.PreviousLaserMessage.X;
            int currentYPosition = LaserConnectionLogic.PreviousLaserMessage.Y;

            Point lowestPoint = point1.Y < point2.Y ? point1 : point2;

            double yDifferenceCurrentAndNew = currentYPosition - newMessage.Y;
            double xDifferenceCurrentAndNew = newMessage.X - currentXPosition;
            double c1 = yDifferenceCurrentAndNew * newMessage.X + xDifferenceCurrentAndNew * newMessage.Y;
            double a2 = point2.Y - point1.Y;
            double crossedLineLengthDifference = point1.X - point2.X;
            double c2 = a2 * lowestPoint.X + crossedLineLengthDifference * lowestPoint.Y;
            double determinant = yDifferenceCurrentAndNew * crossedLineLengthDifference - a2 * xDifferenceCurrentAndNew;
            if (determinant == 0)
            {   // lines do not cross
                return new Point(-4001, -4001);
            }

            double x = (crossedLineLengthDifference * c1 - xDifferenceCurrentAndNew * c2) / determinant;
            double y = (yDifferenceCurrentAndNew * c2 - a2 * c1) / determinant;
            return new Point(x.ToInt(), y.ToInt());
        }
    }
}
