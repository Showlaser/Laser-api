namespace LaserAPI.Models.ToFrontend.LasershowGenerator
{
    public class LaserGeneratorStatusViewmodel
    {
        public bool IsActive { get; set; }
        public string ActiveGenre { get; set; }
        public int? BeatsPerMinute { get; set; }

        public LaserGeneratorStatusViewmodel(bool isActive, string activeGenre, int? beatsPerMinute)
        {
            IsActive = isActive;
            ActiveGenre = activeGenre;
            BeatsPerMinute = beatsPerMinute;
        }
    }
}
