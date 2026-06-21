using LaserAPI.Enums;
using System;

namespace LaserAPI.Models.FromLaser
{
    public class RegisteredLaserViewModel
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public LaserModel ModelType { get; set; }
        public LaserStatus Status { get; set; }
        public string IPAddress { get; set; }
        public byte MaxPowerPerlaserInPercentage { get; set; }
        public byte ProjectionTopInPercentage { get; set; }
        public byte ProjectionBottomInPercentage { get; set; }
        public byte ProjectionLeftInPercentage { get; set; }
        public byte ProjectionRightInPercentage { get; set; }
    }
}
