using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ArcanoidGame.GameObjects;
using ArcanoidGame.Levels;

namespace ArcanoidGame
{
    public class Game
    {
        private readonly LevelsCollection _levelsCollection;
        private Level _level;
        private Ball _ball;
        private Platform _platform;
        private List<Brick> _bricks;  

        private Graphics _graphics;
        private Rectangle _window;

        private int _score;
        private int _bestScore;
        private bool _gameIsOver;

        public bool IsPaused { get; private set; }

        public Game()
        {
            _levelsCollection = new LevelsCollection();
        }

        public void ShowInfo()
        {
            string text = $"Player score {_score}\nBest score {_bestScore}\nLevel \"{_level.Name}\"";
            Font drawFont = new Font("Arial", 15);
            SizeF textSize = _graphics.MeasureString(text, drawFont);
            Point position = new Point(0, 0);
            _graphics.DrawString(text, drawFont, new SolidBrush(Color.AliceBlue), position);
        }

        public void LoadLevel()
        {
            _level = _gameIsOver || _level == null ?
                _levelsCollection.LoadFirstLevel() :
                _levelsCollection.LoadNextLevel();
            _level.Create(_window);
            _ball = _level.Ball;
            _platform = _level.Platform;
            _bricks = _level.Bricks;

            if (_gameIsOver)
            {
                _score = 0;
            }

            _ball.GameOver += (sender, args) => _gameIsOver = true;
        }

        public void TogglePause(bool isPaused)
        {
            if (_gameIsOver)
            {
                if (_score > _bestScore)
                {
                    _bestScore = _score;
                }

                LoadLevel();
                _gameIsOver = false;
                return;
            }

            IsPaused = !IsPaused;
        }

        public void Update(Graphics g, Rectangle window)
        {
            _graphics = g;
            _window = window;

            if (_level == null)
            {
                LoadLevel();
            }

            if (_gameIsOver)
            {
                ShowGameOverMessage();
                return;
            }

            if (IsPaused)
            {
                ShowPauseMessage();
                return;
            }

            _ball.Update(g, window);
            _platform.Update(g, window);
            _bricks.ForEach(b =>
            {
                if (!b.IsSmashed)
                {
                    b.Update(g, window);
                }
            });

            _ball.ResolvePlatformCollisions(_platform);
            _ball.ResolveBricksCollisions(_bricks);

            _bricks.RemoveAll(b =>
            {
                _score += b.IsSmashed ? 100 : 0;
                return b.IsSmashed;
            });

            if (!_bricks.Any())
            {
                LoadLevel();
            }

            ShowInfo();
        }

        private void ShowGameOverMessage()
        {
            string text = @"GAME OVER";
            Font drawFont = new Font("Arial", 60);
            SizeF textSize = _graphics.MeasureString(text, drawFont);
            Point position = new Point(
                (int)((float)_window.Width / 2 - textSize.Width / 2),
                (int)((float)_window.Height / 2 - textSize.Height / 2)
                );
            _graphics.DrawString(text, drawFont, new SolidBrush(Color.Crimson), position);
        }

        private void ShowPauseMessage()
        {
            string text = @"PAUSED";
            Font drawFont = new Font("Arial", 60);
            SizeF textSize = _graphics.MeasureString(text, drawFont);
            Point position = new Point(
                (int) ((float) _window.Width/2 - textSize.Width/2),
                (int) ((float) _window.Height/2 - textSize.Height/2)
                );
            _graphics.DrawString(text, drawFont, new SolidBrush(Color.DeepSkyBlue), position);
        }

        public void MovePlatformRight()
        {
            _platform?.Accelerate(2);
        }

        public void MovePlatformLeft()
        {
            _platform?.Accelerate(-2);
        }
    }
}
