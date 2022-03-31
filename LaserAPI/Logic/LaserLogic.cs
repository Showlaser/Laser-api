using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using LaserAPI.Models.Helper.Laser;
using LaserAPI.Models.Helper.Zones;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LaserLogic
    {
        private readonly List<ZonesHitData> _zones;
        private readonly int _zonesLength;
        private readonly bool _safetyZonesNotActive;
        private readonly ZoneDto _zoneWithHighestLaserPowerPwm;

        public LaserLogic(IZoneDal zoneDal)
        {
            List<ZoneDto> zones = zoneDal
                .All()
                .Result;

            List<ZonesHitData> zonesHitDataCollection = new();
            foreach (ZoneDto zone in zones)
            {
                zonesHitDataCollection.Add(new ZonesHitData
                {
                    Zone = zone,
                    ZoneAbsolutePositions = new ZoneAbsolutePositionsHelper(zone),
                });
            }
            _zones = zonesHitDataCollection;
            _safetyZonesNotActive = !zones.Any();
            _zonesLength = _zones.Count;
            _zoneWithHighestLaserPowerPwm = ZonesHelper.GetZoneWhereMaxLaserPowerPwmIsHighest(_zones.ToList());
        }

        public async Task SendData(LaserMessage newMessage)
        {
            int totalLaserPowerPwm = newMessage.RedLaser + newMessage.GreenLaser + newMessage.BlueLaser;
            bool skipSafetyZoneCheck = _safetyZonesNotActive || totalLaserPowerPwm == 0 ||
                                       _zoneWithHighestLaserPowerPwm.MaxLaserPowerInZonePwm >= totalLaserPowerPwm;

            if (skipSafetyZoneCheck)
            {
                await LaserConnectionLogic.SendMessage(newMessage);
                return;
            }

            ZonesHitData zoneHitData = ZonesHelper.GetZoneWherePathIsInside(_zones, _zonesLength, newMessage.X, newMessage.Y);
            bool positionIsInSafetyZone = zoneHitData != null;

            if (positionIsInSafetyZone)
            {
                LaserSafetyHelper.LimitTotalLaserPowerNecessary(ref newMessage, zoneHitData.Zone.MaxLaserPowerInZonePwm);
                await LaserConnectionLogic.SendMessage(newMessage);
                return;
            }

            List<ZonesHitData> zonesCrossedData =
                    ZonesHelper.GetZonesInPathOfPosition(_zones, LaserConnectionLogic.PreviousLaserMessage.X, LaserConnectionLogic.PreviousLaserMessage.Y,
                        newMessage.X, newMessage.Y);

            int zonesCrossedDataCount = zonesCrossedData.Count;
            if (zonesCrossedDataCount != 0)
            {
                IReadOnlyList<LaserMessage> sortedMessages = GetMessagesOnZonesEdge(newMessage,
                    zonesCrossedData, zonesCrossedDataCount);

                int messagesToSendCount = sortedMessages.Count;
                for (int i = 0; i < messagesToSendCount; i++)
                {
                    LaserMessage messageToSend = sortedMessages[i];
                    await LaserConnectionLogic.SendMessage(messageToSend);
                }
                return;
            }

            await LaserConnectionLogic.SendMessage(newMessage);
        }

        public static IReadOnlyList<LaserMessage> GetMessagesOnZonesEdge(LaserMessage originalMessage,
            List<ZonesHitData> zonesCrossedData, int zonesCrossedCount)
        {
            int totalLaserPowerPwm = originalMessage.RedLaser + originalMessage.GreenLaser + originalMessage.BlueLaser;
            List<LaserMessage> newLaserMessageCollection = new();
            for (int i = 0; i < zonesCrossedCount; i++)
            {
                ZonesHitData crossedZoneData = zonesCrossedData[i];
                if (totalLaserPowerPwm <= crossedZoneData.Zone.MaxLaserPowerInZonePwm)
                {
                    continue; // skip check if power is lower or equal to the max in safety zone
                }
                
                zonesCrossedData[i].ZoneAbsolutePositions = new ZoneAbsolutePositionsHelper(crossedZoneData.Zone);
                crossedZoneData.ZoneSidesHit.GetCoordinateOfZoneCrossing(originalMessage, crossedZoneData, LaserConnectionLogic.PreviousLaserMessage.X,
                    LaserConnectionLogic.PreviousLaserMessage.Y, ref newLaserMessageCollection);
            }

            return SortMessagesFromFurthestToClosestStartPoint(newLaserMessageCollection, originalMessage, newLaserMessageCollection.Count);
        }

        /// <summary>
        /// Sorts all messagesToSort from furthest to the closest newMessage to the startpoint
        /// </summary>
        /// <param name="messagesToSort">The messagesToSort to sort</param>
        /// <param name="startPoint"></param>
        /// <param name="messageCount"></param>
        /// <returns></returns>
        public static IReadOnlyList<LaserMessage> SortMessagesFromFurthestToClosestStartPoint(List<LaserMessage> messagesToSort,
            LaserMessage startPoint, int messageCount)
        {
            if (messageCount == 1)
            {
                return messagesToSort;
            }

            int differenceInYAxis = NumberHelper.GetHighestNumber(startPoint.Y, LaserConnectionLogic.PreviousLaserMessage.X) -
                                    NumberHelper.GetLowestNumber(startPoint.Y, LaserConnectionLogic.PreviousLaserMessage.X);
            int differenceInXAxis = NumberHelper.GetHighestNumber(startPoint.X, LaserConnectionLogic.PreviousLaserMessage.X) -
                                    NumberHelper.GetLowestNumber(startPoint.X, LaserConnectionLogic.PreviousLaserMessage.X);

            bool checkByXAxis = differenceInXAxis > differenceInYAxis;
            List<LaserMessage> sortedMessages = new();
            for (int i = 0; i < messageCount; i++)
            {
                int closestMessageIndex = GetMessageIndexClosestToPoint(messagesToSort, messageCount, checkByXAxis);
                LaserMessage messageToAdd = messagesToSort[closestMessageIndex];
                sortedMessages.Add(messageToAdd);

                messagesToSort.RemoveAt(closestMessageIndex);
                messageCount--;
            }

            sortedMessages.Add(startPoint);
            return sortedMessages;
        }

        /// <summary>
        /// Gets the index of the messagesToSort that is closest to the to the last point
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="messageLength"></param>
        /// <param name="checkByXAxis"></param>
        /// <returns>The index of the closest messagesToSort</returns>
        public static int GetMessageIndexClosestToPoint(IReadOnlyList<LaserMessage> messages, int messageLength, bool checkByXAxis)
        {
            int closestMessageIndex = 0;
            for (int i = 1; i < messageLength; i++) // start at 1 since first element is already used
            {
                LaserMessage message = messages[i];
                LaserMessage closestMessage = messages[closestMessageIndex];

                bool xAxisClosest = checkByXAxis && LaserConnectionLogic.PreviousLaserMessage.X - message.X <
                    LaserConnectionLogic.PreviousLaserMessage.X - closestMessage.X;
                bool yAxisClosest = !checkByXAxis && LaserConnectionLogic.PreviousLaserMessage.Y - message.Y <
                    LaserConnectionLogic.PreviousLaserMessage.Y - closestMessage.Y;

                if (xAxisClosest || yAxisClosest)
                {
                    closestMessageIndex = i;
                }
            }

            return closestMessageIndex;
        }
    }
}
