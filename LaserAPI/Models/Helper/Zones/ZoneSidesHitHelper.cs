using LaserAPI.Models.Helper.Laser;

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

        /// <summary>
        /// Method to calculate the missing axis on the collision between the laser message coordinates and the zone
        /// The method sets the results in the properties LeftYAxis RightYAxis TopXAxis BottomXAxis
        /// </summary>
        /// <param name="message"></param>
        /// <param name="crossedZoneData"></param>
        /// <param name="lastXPosition"></param>
        /// <param name="lastYPosition"></param>
        public void GetCoordinateOfZoneCrossing(LaserMessage message, ZonesHitDataHelper crossedZoneData, int lastXPosition, int lastYPosition)
        {
            ZoneAbsolutePositionsHelper zoneAbsolutePositions = new(crossedZoneData.Zone);

            if (crossedZoneData.ZoneSidesHit.TopHit)
            {
                int yAxisHit = zoneAbsolutePositions.HighestYAxisInZone;
                TopXAxis = CalculateSideXAxis(message, yAxisHit, lastXPosition, lastYPosition);
            }
            if (crossedZoneData.ZoneSidesHit.BottomHit)
            {
                int yAxisHit = zoneAbsolutePositions.LowestYAxisInZone;
                BottomXAxis = CalculateSideXAxis(message, yAxisHit, lastXPosition, lastYPosition);
            }
            if (crossedZoneData.ZoneSidesHit.LeftHit)
            {
                int leftXAxis = zoneAbsolutePositions.LeftXAxisInZone;
                LeftYAxis = CalculateSideYAxis(message, leftXAxis, lastXPosition, lastYPosition);
            }
            if (crossedZoneData.ZoneSidesHit.RightHit)
            {
                int rightXAxis = zoneAbsolutePositions.RightXAxisInZone;
                RightYAxis = CalculateSideYAxis(message, rightXAxis, lastXPosition, lastYPosition);
            }
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
