namespace LaserAPI.Models.ToFrontend.Dashboard
{
    public class LaserSettingsViewmodel
    {
        public int ZonesLength { get; set; }
        public bool DevelopmentModeIsActive { get; set; }

        public LaserSettingsViewmodel(int zonesLength, bool developmentModeIsActive)
        {
            ZonesLength = zonesLength;
            DevelopmentModeIsActive = developmentModeIsActive;
        }
    }
}
