using System;

namespace LaserAPI.Models.ToFrontend.Pattern
{
    public class PointsViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid PatternUuid { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int RedLaserPowerPwm { get; set; }
        public int GreenLaserPowerPwm { get; set; }
        public int BlueLaserPowerPwm { get; set; }
        public int ConnectedToPointOrderNr { get; set; }
        public int OrderNr { get; set; }
    }
}
