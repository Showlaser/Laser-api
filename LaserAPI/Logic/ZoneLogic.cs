using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using LaserAPI.Models.Helper.Zones;
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
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <returns>The zones that are crossed</returns>
        private static List<ZoneLine> GetLineHitByPath(ZoneDto zone, int newX,
            int newY)
        {
            int previousX = LaserConnectionLogic.PreviousLaserMessage.X;
            int previousY = LaserConnectionLogic.PreviousLaserMessage.Y;

            return null;
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
        /// <returns>A new point with the x and y position of the crossing point</returns>
        public static Point CalculateCrossingPointOfTwoLines(LaserMessage newMessage, Point point1, Point point2)
        {
            int currentXPosition = LaserConnectionLogic.PreviousLaserMessage.X;
            int currentYPosition = LaserConnectionLogic.PreviousLaserMessage.Y;

            Point lowestPoint = point1.Y < point2.Y ? point1 : point2;

            double yDifferenceCurrentAndNew = currentYPosition - newMessage.Y;
            double xDifferenceCurrentAndNew = newMessage.X - currentXPosition;
            double c1 = yDifferenceCurrentAndNew * newMessage.X + xDifferenceCurrentAndNew * newMessage.Y;
            double a2 =  point2.Y - point1.Y;
            double crossedLineLengthDifference = point1.X - point2.X;
            double c2 = a2 * lowestPoint.X + crossedLineLengthDifference * lowestPoint.Y;
            double determinant = yDifferenceCurrentAndNew * crossedLineLengthDifference - a2 * xDifferenceCurrentAndNew;
            
            double x = (crossedLineLengthDifference * c1 - xDifferenceCurrentAndNew * c2) / determinant;
            double y = (yDifferenceCurrentAndNew * c2 - a2 * c1) / determinant;
            return new Point(x.ToInt(), y.ToInt());
        }
    }
}
