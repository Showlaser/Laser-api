using LaserAPI.Enums;
using LaserAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Logic.Game
{
    public class GameLogic
    {
        private readonly List<IGame> _playableGames = new();
        private IGame _selectedGame;

        public GameLogic()
        {
            List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(e => e.GetTypes())
                .Where(e => typeof(IGame).IsAssignableFrom(e) && e.IsClass)
                .ToList();

            _playableGames.AddRange(types.Select(t => (IGame)Activator.CreateInstance(t)));
        }

        public List<string> GetGameNames()
        {
            return _playableGames.Select(pg => pg.GetName())
                .ToList();
        }

        public void Start(string gameName)
        {
            if (_selectedGame != null && !_selectedGame.GameOver())
            {
                throw new InvalidOperationException("The game was not stopped before calling start");
            }

            _selectedGame = _playableGames.FirstOrDefault(pg => pg.GetName() == gameName);
            if (_selectedGame == null)
            {
                throw new KeyNotFoundException();
            }

            _selectedGame.Start();
        }

        public void Stop()
        {
            _selectedGame.Stop();
            _selectedGame = null;
        }

        public void Move(GameMovement movement)
        {
            _selectedGame.Move(movement);
        }
    }
}
