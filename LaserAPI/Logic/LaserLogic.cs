using LaserAPI.Interfaces;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.FromFrontend.Points;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LaserLogic(ZoneLogic zoneLogic, ILaserConnectionLogic laserConnectionLogic)
    {
        public static async Task SendData(IReadOnlyList<LaserMessage> messages, int duration)
        {
            List<LaserMessage> messagesToSend = [];

            int messagesLength = messages.Count;
            for (int i = 0; i < messagesLength; i++)
            {
                LaserMessage message = messages[i];
                LaserMessage previousMessage = i == 0 ? message : messages[i - 1];

                SafetyZoneDto zoneWherePathIsInside = ProjectionZonesLogic.GetZoneWherePathIsInside(message, previousMessage);
                bool positionIsInProjectionZone = zoneWherePathIsInside != null;

                if (positionIsInProjectionZone)
                {
                    LimitTotalLaserPowerIfNecessary(ref message, zoneWherePathIsInside.MaxLaserPowerInZonePercentage);
                    messagesToSend.Add(message);
                }

                LaserMessage[] zoneCrossingPoints = ProjectionZonesLogic.GetPointsOfZoneLinesHitByPath(message, previousMessage);
                if (zoneCrossingPoints.Length > 0)
                {
                    messagesToSend.AddRange(zoneCrossingPoints);
                }
            }
        }

        /// <summary>
        /// Limits the pwm laser power if the value is greater than the max power
        /// </summary>
        /// <param name="message">The message to modify</param>
        /// <param name="maxPowerPwm">The max power allowed</param>
        private static void LimitTotalLaserPowerIfNecessary(ref LaserMessage message, int maxPowerPwm)
        {
            int combinedPower = message.RedLaser + message.GreenLaser + message.BlueLaser;
            if (combinedPower > maxPowerPwm)
            {
                double redLaserPowerPercentage = message.RedLaser.ToDouble() / combinedPower.ToDouble();
                double greenLaserPowerPercentage = message.GreenLaser.ToDouble() / combinedPower.ToDouble();
                double blueLaserPowerPercentage = message.BlueLaser.ToDouble() / combinedPower.ToDouble();

                message.RedLaser = Math.Floor(maxPowerPwm * redLaserPowerPercentage).ToInt();
                message.GreenLaser = Math.Floor(maxPowerPwm * greenLaserPowerPercentage).ToInt();
                message.BlueLaser = Math.Floor(maxPowerPwm * blueLaserPowerPercentage).ToInt();
            }
        }

        /// <summary>
        /// The provided points from the front end
        /// </summary>
        /// <param name="points">The points to Render</param>
        public async Task RenderProvidedPoints(List<PointWrapper> wrappedPoints)
        {
            List<PointWrapper> sortedPoints = wrappedPoints.OrderBy(p => p.PatternPoint.OrderNr).ToList();
            List<PointWrapper> pointsToRender = [];

            int pointsLength = sortedPoints.Count;
            for (int i = 0; i < pointsLength; i++)
            {
                PointWrapper point = sortedPoints.ElementAt(i);
                pointsToRender.Add(point);

                bool dotsFormALine = point.PatternPoint.ConnectedToPointUuid != null && point.PatternPoint.ConnectedToPointUuid != Guid.Empty;
                if (dotsFormALine)
                {
                    PointWrapper PointToConnectTo = sortedPoints.Single(sp => point.PatternPoint.ConnectedToPointUuid == sp.PatternPoint.Uuid);
                    pointsToRender.Add(PointToConnectTo);
                }
            }

            await laserConnectionLogic.SendData(pointsToRender);
        }
    }
}
