using LaserAPI.Models.Dto.Animations;

namespace LaserAPI.Models.Helper.LaserAnimations
{
    public interface IPreMadeLaserAnimation
    {
        public string AnimationName { get; }
        public AnimationDto GetAnimation(PreMadeAnimationOptions options);
    }
}
