using LaserAPI.Enums;

namespace LaserAPI.Interfaces
{
    public interface IGame
    {
        string GetName();
        bool GameOver();
        void Start();
        void Stop();
        void Move(GameMovement movement);
    }
}
