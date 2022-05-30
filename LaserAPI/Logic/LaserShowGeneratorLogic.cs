namespace LaserAPI.Logic
{
    public class LaserShowGeneratorLogic
    {
        public LaserShowGeneratorAlgorithm LaserShowGeneratorAlgorithm { get; }

        public LaserShowGeneratorLogic(LaserShowGeneratorAlgorithm laserShowGeneratorAlgorithm, AnimationLogic animationLogic)
        {
            LaserShowGeneratorAlgorithm = laserShowGeneratorAlgorithm;
            LaserShowGeneratorAlgorithm.AnimationLogic = animationLogic;
        }
    }
}
