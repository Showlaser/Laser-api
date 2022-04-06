using LaserAPI.Enums;
using LaserAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using LaserAPI.Interfaces.Dal;

namespace LaserAPI.Logic.Game
{
    public class GameLogic
    {
        private readonly GameStateLogic _gameStateLogic;
        private readonly List<IGame> _playableGames = new();

        public GameLogic(AnimationLogic animationLogic, GameStateLogic gameStateLogic)
        {
            _gameStateLogic = gameStateLogic;
            List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(e => e.GetTypes())
                .Where(e => typeof(IGame).IsAssignableFrom(e) && e.IsClass)
                .ToList();
                
            List<object> paramArray = new() { animationLogic };
            _playableGames.AddRange(types.Select(t => (IGame)Activator.CreateInstance(t, paramArray.ToArray())));
        }

        public List<string> GetGameNames()
        {
            return _playableGames.Select(pg => pg.GetName())
                .ToList();
        }

        public void Start(string gameName)
        {
            if (_gameStateLogic.SelectedGame != null && !_gameStateLogic.SelectedGame.GameOver())
            {
                throw new InvalidOperationException("The game was not stopped before calling start");
            }

            _gameStateLogic.SelectedGame = _playableGames.FirstOrDefault(pg => pg.GetName() == gameName);
            if (_gameStateLogic.SelectedGame == null)
            {
                throw new KeyNotFoundException();
            }

            _gameStateLogic.SelectedGame.Start();
        }

        public void Stop()
        {
            _gameStateLogic.SelectedGame.Stop();
            _gameStateLogic.SelectedGame = null;
        }

        public void Move(GameMovement movement)
        {
            _gameStateLogic.SelectedGame.Move(movement);
        }
    }
}
