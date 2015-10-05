using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidGame.Levels
{
    public class LevelsCollection
    {
        private readonly List<Level> _levels;
        private int _currentIndex;

        public LevelsCollection()
        {
            _currentIndex = -1;

            _levels = new List<Level>
            {
                new Level(
                    name: "First",
                    ballSize: 20,
                    ballBottomOffset: 100,
                    ballMaxAcceleration: 10,
                    platformWidth: 200,
                    platformHeight: 10,
                    platformBottomOffset: 20,
                    platformMaxAcceleration: 40,
                    brickWidth: 90,
                    brickHeight: 20,
                    bricksMapOffset: 300,
                    bricksMap: new[] {4}),
                new Level(
                    name: "Second",
                    ballSize: 20,
                    ballBottomOffset: 100,
                    ballMaxAcceleration: 10,
                    platformWidth: 100,
                    platformHeight: 10,
                    platformBottomOffset: 20,
                    platformMaxAcceleration: 20,
                    brickWidth: 90,
                    brickHeight: 20,
                    bricksMapOffset: 300,
                    bricksMap: new[] {2, 4, 0, 6, 8})
            };

        }

        public Level LoadCurrentLevel()
        {
            return _levels[_currentIndex];
        }

        public Level LoadFirstLevel()
        {
            _currentIndex = 0;
            return _levels[0];
        }

        public Level LoadNextLevel()
        {
            _currentIndex++;

            if (_currentIndex > _levels.Count - 1)
            {
                return LoadFirstLevel();
            }

            return _levels[_currentIndex];
        }
    }
}
