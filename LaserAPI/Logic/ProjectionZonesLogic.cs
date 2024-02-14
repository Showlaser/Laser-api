using LaserAPI.CustomExceptions;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LaserAPI.Logic
{
    public static class ProjectionZonesLogic
    {
        private static int _zonesLength;
        private static List<ZoneDto> _zones;

        public static List<ZoneDto> Zones
        {
            set
            {
                _zonesLength = value.Count;
                _zones = value;
            }
        }

        /// <summary>
        /// Gets the zone where the laser path is in, if the path is not within a zone null is returned
        /// </summary>
        /// <param name="message">The laser message</param>
        /// <param name="previousMessage">The previous message send</param>
        /// <returns>The zone where the laser path is in, if the path is not within a zone null is returned</returns>
        public static ZoneDto GetZoneWherePathIsInside(LaserMessage message, LaserMessage previousMessage)
        {
            for (int i = 0; i < _zonesLength; i++)
            {
                ZoneDto zone = _zones[i];

                List<Point> zonePoints = new();
                int zonePointLength = zone.Points.Count;
                for (int j = 0; j < zonePointLength; j++)
                {
                    ZonesPositionDto zonePoint = zone.Points[j];
                    zonePoints.Add(new Point(zonePoint.X, zonePoint.Y));
                }

                if (IsInsidePolygon(zonePoints, new Point(previousMessage.X, previousMessage.Y)) &&
                    IsInsidePolygon(zonePoints, new Point(message.X, message.Y)))
                {
                    return zone;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all the points of the lines that form a zone that are hit with the laser path, points are sorted from closest to previous location
        /// </summary>
        /// <param name="message">The new location for the laser to go to</param>
        /// <param name="previousMessage"></param>
        /// <returns>The points of the lines that form a zone that are hit with the laser path in a laser message model</returns>
        public static LaserMessage[] GetPointsOfZoneLinesHitByPath(LaserMessage message, LaserMessage previousMessage)
        {
            if (_zonesLength == 0)
            {
                throw new NoProjectionZonesSetException();
            }

            List<LaserMessage> crossingPoints = new();
            for (int i = 0; i < _zonesLength; i++)
            {
                ZoneDto zone = _zones[i];
                ZoneLine[] zoneLinesHit = GetZoneLineHitByPath(zone, message, previousMessage);

                int zoneLinesHitLength = zoneLinesHit.Length;
                LaserMessage messageWithLimitedPower = new(message.RedLaser, message.GreenLaser, message.BlueLaser, 0, 0);
                LaserLogic.LimitTotalLaserPowerIfNecessary(ref messageWithLimitedPower, zone.MaxLaserPowerInZonePwm);

                for (int j = 0; j < zoneLinesHitLength; j++)
                {
                    Point point = zoneLinesHit[j].CrossedPoint;
                    LaserMessage messageToAdd = new(messageWithLimitedPower.RedLaser, messageWithLimitedPower.GreenLaser, messageWithLimitedPower.BlueLaser, point.X, point.Y);
                    crossingPoints.Add(messageToAdd);
                    if (j == 0)
                    {
                        crossingPoints.Add(new LaserMessage(0, 0, 0, point.X, point.Y));
                    }
                }
            }

            return SortPointsFromClosestToPreviousSendMessageToFarthest(crossingPoints, previousMessage);
        }

        /// <summary>
        /// Checks if the path of the previous position and the new position crosses a zone and returns the data of the crossed zone line
        /// </summary>
        /// <param name="zone">The zone to check</param>
        /// <param name="message">The new location for the laser</param>
        /// <returns>The zones that are crossed</returns>
        public static ZoneLine[] GetZoneLineHitByPath(ZoneDto zone, LaserMessage message, LaserMessage previousMessage)
        {
            int zonePositionsLength = zone.Points.Count;
            List<ZoneLine> points = new(zonePositionsLength);

            for (int i = 0; i < zonePositionsLength; i++)
            {
                // these two positions form a connected line
                ZonesPositionDto position = zone.Points[i];
                int secondZoneIndex = i == zonePositionsLength - 1 ? 0 : i + 1;
                ZonesPositionDto secondPosition = zone.Points[secondZoneIndex];

                Point zonePoint1 = new(position.X, position.Y);
                Point zonePoint2 = new(secondPosition.X, secondPosition.Y);

                Point crossedPoint = CalculateCrossingPointOfZoneLineAndLaserPath(message, previousMessage, zonePoint1, zonePoint2);
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

            return points.DistinctBy(p => p.CrossedPoint).ToArray();
        }

        /// <summary>
        /// Sorts the points from closest to farthest away from the previous point 
        /// </summary>
        /// <param name="points">The points to sort</param>
        /// <returns>The given points sorted from closest to farthest away from the previous send point</returns>
        public static LaserMessage[] SortPointsFromClosestToPreviousSendMessageToFarthest(List<LaserMessage> points, LaserMessage previousMessage)
        {
            if (points.Count == 0)
            {
                return Array.Empty<LaserMessage>();
            }

            DistanceSorter[] distances = new DistanceSorter[points.Count];
            int pointsLength = points.Count;
            for (int i = 0; i < pointsLength; i++)
            {
                LaserMessage message = points[i];
                int totalLaserPower = message.RedLaser + message.GreenLaser + message.BlueLaser;
                double distance = Math.Sqrt(Math.Pow(previousMessage.X - message.X, 2) + Math.Pow(previousMessage.Y - message.Y, 2));
                distances[i] = new DistanceSorter(message, distance, totalLaserPower);
            }

            DistanceSorter[] sortedDistances = distances.OrderBy(d => d.Distance)
                 .ThenByDescending(d => d.TotalLaserPower)
                 .ToArray();

            LaserMessage[] sorted = new LaserMessage[pointsLength];
            for (int i = 0; i < pointsLength; i++)
            {
                LaserMessage sortedMessage = sortedDistances[i].Message;
                sorted[i] = sortedMessage;
            }

            return sorted;
        }

        // Given three collinear points thePointToCheck, q, r, the function checks if
        // point q lies on line segment 'pr'
        private static bool OnSegment(Point p, Point q, Point r) =>
            q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) && q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);

        // To find orientation of ordered triplet (thePointToCheck, q, r).
        // The function returns following values
        // 0 --> thePointToCheck, q and r are collinear
        // 1 --> Clockwise
        // 2 --> Counterclockwise
        private static int Orientation(Point p, Point q, Point r)
        {
            // See https://www.geeksforgeeks.org/orientation-3-ordered-points/
            // for details of below formula.
            int val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0)
            {
                return 0; // collinear
            }

            return val > 0 ? 1 : 2; // clock or counter clock wise
        }

        // The main function that returns true if line segment 'p1q1'
        // and 'p2q2' intersect.
        private static bool DoIntersect(Point p1, Point q1, Point p2, Point q2)
        {
            // Find the four orientations needed for general and
            // special cases
            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);

            // General case
            if (o1 != o2 && o3 != o4)
            {
                return true;
            }
            // Special Cases
            // p1, q1 and p2 are collinear and p2 lies on segment p1q1
            if (o1 == 0 && OnSegment(p1, p2, q1))
            {
                return true;
            }
            // p1, q1 and q2 are collinear and q2 lies on segment p1q1
            if (o2 == 0 && OnSegment(p1, q2, q1))
            {
                return true;
            }
            // p2, q2 and p1 are collinear and p1 lies on segment p2q2
            if (o3 == 0 && OnSegment(p2, p1, q2))
            {
                return true;
            }
            // p2, q2 and q1 are collinear and q1 lies on segment p2q2
            if (o4 == 0 && OnSegment(p2, q1, q2))
            {
                return true;
            }

            return false;
        }

        // Returns true if the point thePointToCheck lies
        // inside the polygonPositions[] with polygonLength vertices
        public static bool IsInsidePolygon(IReadOnlyList<Point> polygonPositions, Point thePointToCheck)
        {
            // There must be at least 3 vertices in polygonPositions[]
            int polygonLength = polygonPositions.Count;
            if (polygonLength < 3)
            {
                return false;
            }

            // Create a point for line segment from thePointToCheck to infinite
            Point extreme = new(10000, thePointToCheck.Y);

            // Count intersections of the above line
            // with sides of polygonPositions
            int count = 0, i = 0;
            do
            {
                int next = (i + 1) % polygonLength;

                // Check if the line segment from 'thePointToCheck' to
                // 'extreme' intersects with the line
                // segment from 'polygonPositions[i]' to 'polygonPositions[next]'
                if (DoIntersect(polygonPositions[i],
                        polygonPositions[next], thePointToCheck, extreme))
                {
                    // If the point 'thePointToCheck' is collinear with line
                    // segment 'i-next', then check if it lies
                    // on segment. If it lies, return true, otherwise false
                    if (Orientation(polygonPositions[i], thePointToCheck, polygonPositions[next]) == 0)
                    {
                        return OnSegment(polygonPositions[i], thePointToCheck,
                            polygonPositions[next]);
                    }
                    count++;
                }
                i = next;
            } while (i != 0);

            // Return true if count is odd, false otherwise
            return (count % 2 == 1); // Same as (count%2 == 1)
        }

        /// <summary>
        /// This method calculates the crossing x and y point with two lines
        /// </summary>
        /// <param name="newMessage">The new position</param>
        /// <param name="previousMessage">The previous message send</param>
        /// <param name="line2Point1">The first point of the line</param>
        /// <param name="line2Point2">The second point of the line</param>
        /// <returns>A new point with the x and y position of the crossing point if the lines do not cross a point with the values -4001, -4001 is returned</returns>
        public static Point CalculateCrossingPointOfZoneLineAndLaserPath(LaserMessage newMessage, LaserMessage previousMessage, Point line2Point1, Point line2Point2)
        {
            int currentXPosition = previousMessage.X;
            int currentYPosition = previousMessage.Y;
            bool linesDoNotCross = !DoIntersect(new Point(newMessage.X, newMessage.Y),
                new Point(currentXPosition, currentYPosition), line2Point1, line2Point2);

            if (linesDoNotCross)
            {
                return new Point(-4001, -4001);
            }

            Point lowestPoint = line2Point1.Y < line2Point2.Y ? line2Point1 : line2Point2;

            double yDifferenceCurrentAndNew = currentYPosition - newMessage.Y;
            double xDifferenceCurrentAndNew = newMessage.X - currentXPosition;
            double c1 = yDifferenceCurrentAndNew * newMessage.X + xDifferenceCurrentAndNew * newMessage.Y;
            double a2 = line2Point2.Y - line2Point1.Y;
            double crossedLineLengthDifference = line2Point1.X - line2Point2.X;
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
