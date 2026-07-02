using LaserAPI.Models.Dto.RegisteredLaser;

namespace LaserAPI.Models.FromFrontend.Showlaser.SDCard
{
    public class SDCardJsonFileWrapper
    {
        public RegisteredLaserDto RegisteredLaser { get; set; }
        public string Filename { get; set; }
        public string FileJson{ get; set; }
    }
}
