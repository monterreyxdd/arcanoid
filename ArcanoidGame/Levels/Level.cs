using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ArcanoidGame.GameObjects;

namespace ArcanoidGame.Levels
{
    public class Level
    {
        public string Name { get; }

        public int BallSize { get; }
        public int BallBottomOffset { get; }
        public int BallMaxAcceleration { get; }

        public int PlatformWidth { get; }
        public int PlatformHeight { get; }
        public int PlatformBottomOffset { get; }
        public int PlatformMaxAcceleration { get; }

        public int BrickWidth { get; }
        public int BrickHeight { get; }

        public int BricksMapOffset { get; set; }
        public int[] BricksMap { get; }

        public Ball Ball { get; private set; }
        public Platform Platform { get; private set; }
        public List<Brick> Bricks { get; private set; }

        public Level(string name, int ballSize, int ballBottomOffset, int ballMaxAcceleration, int platformWidth,
            int platformHeight, int platformBottomOffset, int platformMaxAcceleration, int brickWidth, int brickHeight, int bricksMapOffset,
            int[] bricksMap)
        {
            Name = name;
            BallSize = ballSize;
            BallBottomOffset = ballBottomOffset;
            BallMaxAcceleration = ballMaxAcceleration;
            PlatformWidth = platformWidth;
            PlatformHeight = platformHeight;
            PlatformBottomOffset = platformBottomOffset;
            PlatformMaxAcceleration = platformMaxAcceleration;
            BrickWidth = brickWidth;
            BrickHeight = brickHeight;
            BricksMapOffset = bricksMapOffset;
            BricksMap = bricksMap;
        }

        public void Create(Rectangle window)
        {
            Ball = new Ball(BallSize, BallBottomOffset, BallMaxAcceleration);
            Platform = new Platform(PlatformWidth, PlatformHeight, PlatformBottomOffset, PlatformMaxAcceleration);
            Bricks = GenerateBricks(window).ToList();
        }

        private IEnumerable<Brick> GenerateBricks(Rectangle window)
        {
            List<Brick> bricks = new List<Brick>();

            int initialOffsetY = BricksMapOffset;
            int currentY = initialOffsetY;

            foreach (int line in BricksMap)
            {
                if (line%2 == 0)
                {
                    int sideCount = line/2;
                    int sideWidth = sideCount*BrickWidth;
                    int initialOffsetX = window.Width/2 - sideWidth;
                    
                    int currentX = initialOffsetX;
                   
                    for (int i = 0; i < line; i++)
                    {
                        bricks.Add(new Brick(currentX, currentY, BrickWidth, BrickHeight));
                        currentX += BrickWidth;
                    }

                    currentY += BrickHeight;
                }
            }

            return bricks;
        }
    }
}