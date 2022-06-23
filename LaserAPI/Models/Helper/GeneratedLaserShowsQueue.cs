using LaserAPI.Models.Dto.Animations;

namespace LaserAPI.Models.Helper
{
    /// <summary>
    /// This class is to store a lasershow that is generated. Duo to the architect of this application it is not possible
    /// in the lasershowGeneratorAlgorithm class
    /// </summary>
    public static class GeneratedLaserShowsQueue
    {
        public static AnimationDto LaserShowToSave { get; set; } = new();
    }
}
