using LaserAPI.Models.Helper.Laser;
using System.Collections.Generic;

namespace LaserAPI.Models.Helper.Zones
{
    public class ZoneSidesHitHelper
    {
        public bool LeftHit { get; set; }
        public bool RightHit { get; set; }
        public bool TopHit { get; set; }
        public bool BottomHit { get; set; }

        public int LeftYAxis { get; private set; }
        public int RightYAxis { get; private set; }
        public int TopXAxis { get; private set; }
        public int BottomXAxis { get; private set; }

        public int TotalZoneSidesHit { get; private set; }

        /// <summary>
        /// Method to calculate the missing axis on the collision between the laser message coordinates and the zone
        /// The method sets the results in the properties LeftYAxis RightYAxis TopXAxis BottomXAxis
        /// </summary>
        /// <param name="message"></param>
        /// <param name="crossedZoneData"></param>
        /// <param name="lastXPosition"></param>
        /// <param name="lastYPosition"></param>
        /// <param name="newMessageCollection"></param>
        public void GetMissingXOrYCoordinateOfZoneCrossing(LaserMessage message, ZonesHitData crossedZoneData, int lastXPosition, int lastYPosition,
        ref List<LaserMessage> newMessageCollection)
        {
            ZoneAbsolutePositionsHelper zoneAbsolutePositions = crossedZoneData.ZoneAbsolutePositions;
            if (crossedZoneData.ZoneSidesHit.TopHit)
            {
                OnTopSideHit(message, crossedZoneData, lastXPosition, lastYPosition, newMessageCollection, zoneAbsolutePositions);
            }

            if (crossedZoneData.ZoneSidesHit.BottomHit)
            {
                OnBottomSideHit(message, crossedZoneData, lastXPosition, lastYPosition, newMessageCollection, zoneAbsolutePositions);
            }
            if (crossedZoneData.ZoneSidesHit.LeftHit)
            {
                OnLeftSideHit(message, crossedZoneData, lastXPosition, lastYPosition, newMessageCollection, zoneAbsolutePositions);
            }
            if (crossedZoneData.ZoneSidesHit.RightHit)
            {
                OnRightSideHit(message, crossedZoneData, lastXPosition, lastYPosition, newMessageCollection, zoneAbsolutePositions);
            }
        }

        private void OnRightSideHit(LaserMessage message, ZonesHitData crossedZoneData, int lastXPosition,
            int lastYPosition, ICollection<LaserMessage> newMessageCollection, ZoneAbsolutePositionsHelper zoneAbsolutePositions)
        {
            int rightXAxis = zoneAbsolutePositions.RightXAxisInZone;
            RightYAxis = CalculateSideYAxis(message, rightXAxis, lastXPosition, lastYPosition);
            TotalZoneSidesHit++;

            LaserMessage messageOnZoneEdge = new()
            {
                X = crossedZoneData.ZoneAbsolutePositions.RightXAxisInZone,
                Y = crossedZoneData.ZoneSidesHit.RightYAxis,
                RedLaser = message.RedLaser,
                GreenLaser = message.GreenLaser,
                BlueLaser = message.BlueLaser,
            };

            LaserSafetyHelper.LimitTotalLaserPowerIfNecessary(ref messageOnZoneEdge, crossedZoneData.Zone.MaxLaserPowerInZonePwm);
            newMessageCollection.Add(messageOnZoneEdge);
        }

        private void OnLeftSideHit(LaserMessage message, ZonesHitData crossedZoneData, int lastXPosition,
            int lastYPosition, ICollection<LaserMessage> newMessageCollection, ZoneAbsolutePositionsHelper zoneAbsolutePositions)
        {
            int leftXAxis = zoneAbsolutePositions.LeftXAxisInZone;
            LeftYAxis = CalculateSideYAxis(message, leftXAxis, lastXPosition, lastYPosition);
            TotalZoneSidesHit++;

            LaserMessage messageOnZoneEdge = new()
            {
                X = crossedZoneData.ZoneAbsolutePositions.LeftXAxisInZone,
                Y = crossedZoneData.ZoneSidesHit.LeftYAxis,
                RedLaser = message.RedLaser,
                GreenLaser = message.GreenLaser,
                BlueLaser = message.BlueLaser,
            };

            LaserSafetyHelper.LimitTotalLaserPowerIfNecessary(ref messageOnZoneEdge, crossedZoneData.Zone.MaxLaserPowerInZonePwm);
            newMessageCollection.Add(messageOnZoneEdge);
        }

        private void OnBottomSideHit(LaserMessage message, ZonesHitData crossedZoneData, int lastXPosition,
            int lastYPosition, ICollection<LaserMessage> newMessageCollection, ZoneAbsolutePositionsHelper zoneAbsolutePositions)
        {
            int yAxisHit = zoneAbsolutePositions.LowestYAxisInZone;
            BottomXAxis = CalculateSideXAxis(message, yAxisHit, lastXPosition, lastYPosition);
            TotalZoneSidesHit++;

            LaserMessage messageOnZoneEdge = new()
            {
                X = crossedZoneData.ZoneSidesHit.BottomXAxis,
                Y = crossedZoneData.ZoneAbsolutePositions.LowestYAxisInZone,
                RedLaser = message.RedLaser,
                GreenLaser = message.GreenLaser,
                BlueLaser = message.BlueLaser,
            };

            LaserSafetyHelper.LimitTotalLaserPowerIfNecessary(ref messageOnZoneEdge, crossedZoneData.Zone.MaxLaserPowerInZonePwm);
            newMessageCollection.Add(messageOnZoneEdge);
        }

        private void OnTopSideHit(LaserMessage message, ZonesHitData crossedZoneData, int lastXPosition,
            int lastYPosition, ICollection<LaserMessage> newMessageCollection, ZoneAbsolutePositionsHelper zoneAbsolutePositions)
        {
            int yAxisHit = zoneAbsolutePositions.HighestYAxisInZone;
            TopXAxis = CalculateSideXAxis(message, yAxisHit, lastXPosition, lastYPosition);
            TotalZoneSidesHit++;

            LaserMessage messageOnZoneEdge = new()
            {
                X = crossedZoneData.ZoneSidesHit.TopXAxis,
                Y = zoneAbsolutePositions.HighestYAxisInZone,
                RedLaser = message.RedLaser,
                GreenLaser = message.GreenLaser,
                BlueLaser = message.BlueLaser,
            };

            LaserSafetyHelper.LimitTotalLaserPowerIfNecessary(ref messageOnZoneEdge, crossedZoneData.Zone.MaxLaserPowerInZonePwm);
            newMessageCollection.Add(messageOnZoneEdge);
        }

        private static int CalculateSideXAxis(LaserMessage message, int yAxisHit, int lastXPosition, int lastYPosition)
        {
            return NumberHelper.Map(yAxisHit, NumberHelper.GetLowestNumber(message.X, lastYPosition),
                NumberHelper.GetHighestNumber(message.Y, lastYPosition),
                NumberHelper.GetLowestNumber(message.X, lastXPosition),
                NumberHelper.GetHighestNumber(message.X, lastXPosition));
        }

        private static int CalculateSideYAxis(LaserMessage message, int leftXAxis, int lastXPosition, int lastYPosition)
        {
            return NumberHelper.Map(leftXAxis, NumberHelper.GetLowestNumber(message.X, lastXPosition),
                NumberHelper.GetHighestNumber(message.X, lastXPosition),
                NumberHelper.GetLowestNumber(message.Y, lastYPosition),
                NumberHelper.GetHighestNumber(message.Y, lastYPosition));
        }
    }
}
