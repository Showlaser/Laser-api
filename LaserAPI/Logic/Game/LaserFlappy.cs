using LaserAPI.Enums;
using LaserAPI.Interfaces;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Helper;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace LaserAPI.Logic.Game
{
    public class LaserFlappy : IGame
    {
        private readonly AnimationLogic _animationLogic;
        private int _score;
        private readonly Timer _timer = new();
        private int _flappyYPosition = 1000;
        private int _obstacleXPosition = 0;
        private int _obstacleHeight = 400;
        private int _obstacleWidth = 400;
        private int _gameSpeed = 1;
        private int _iterations = 0; // game timer in second max value is 30 seconds
        private Task _gameTask;
        private bool _obstacleOnTop;
        private bool _gameOver;

        public LaserFlappy(AnimationLogic animationLogic)
        {
            _animationLogic = animationLogic;
        }

        private async Task DrawScreen()
        {
            await DrawFlappy();
            await DrawObstacle();
        }

        private async Task DrawFlappy()
        {
            int width = 400;
            int height = 400;
            int centerX = -4000 + width;
            int centerY = -4000 + height + _flappyYPosition;

            AnimationDto flappyAnimation = PreMadeAnimations.GetRectangle(width, height, centerX, centerY);
            await AnimationLogic.PlayAnimation(flappyAnimation);
        }

        private async Task DrawObstacle()
        {
            int centerY = _obstacleOnTop ? 4000 - _obstacleHeight : -4000 + _obstacleHeight;

            _obstacleXPosition -= 50 * _gameSpeed;
            if (_obstacleXPosition - _obstacleWidth <= -4000)
            {
                _obstacleHeight = new Random(Guid.NewGuid().GetHashCode()).Next(800, 3500);
                _obstacleWidth = new Random(Guid.NewGuid().GetHashCode()).Next(200, 1500);
                _obstacleOnTop = new Random(Guid.NewGuid().GetHashCode()).Next(0, 2) == 1;

                _obstacleXPosition = 4000;
                _score++;
            }

            AnimationDto obstacleAnimation =
                PreMadeAnimations.GetRectangle(_obstacleWidth, _obstacleHeight, _obstacleXPosition, centerY);
            await AnimationLogic.PlayAnimation(obstacleAnimation);
        }

        public string GetName()
        {
            return "Laser flappy";
        }

        public bool GameOver()
        {
            return _gameOver;
        }

        public void Start()
        {
            _score = 0;
            _timer.Start();
            _timer.Elapsed += TimerOnElapsed;
            _timer.Interval = 16;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            bool taskAvailable = _gameTask == null || _gameTask.IsCompleted;
            _iterations++;
            if (_iterations >= 1000)
            {
                _iterations = 0;
                if (_gameSpeed > 10)
                {
                    return;
                }

                _gameSpeed++;
            }

            if (taskAvailable)
            {
                _gameTask = new Task(DrawGame);
                _gameTask.Start();
            }
        }

        private async void DrawGame()
        {
            if (-4000 + 400 + _flappyYPosition > -4000)
            {
                _flappyYPosition -= 20 * _gameSpeed;
            }
            /*else
                    {
                        Console.WriteLine("Score: " + _score);
                        Debug.WriteLine("Score: " + _score);
                        _gameOver = true;
                        Stop();
                    }*/

            await DrawScreen();
        }

        public void Stop()
        {
            _timer.Stop();
            _gameOver = true;
            _flappyYPosition = 0;
            _score = 0;
            _gameSpeed = 1;
            _iterations = 0;
            _obstacleOnTop = false;
        }

        public void Move(GameMovement movement)
        {
            if (movement == GameMovement.Up && _flappyYPosition - 400 + 500 <= 4000)
            {
                _flappyYPosition += 250 * _gameSpeed;
            }
        }
    }
}
