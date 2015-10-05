using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidGame.GameObjects
{
    public class Platform : GameObjectBase
    {
        public Point Acceleration { get; set; }
        public int MaxAcceleration { get; set; }
        public int BottomOffset { get; set; }

        public Platform(int width, int height, int bottomOffset, int maxAcceleration)
        {
            Size = new Size(width, height);
            Color = Color.Aquamarine;
            Acceleration = new Point(0, 0);
            MaxAcceleration = maxAcceleration;
            BottomOffset = bottomOffset;
        }

        private void SetLocation(Rectangle window)
        {
            Location = new Point(
                window.Width/2 - Size.Width/2,
                window.Height - Size.Height - BottomOffset
                );
        }

        private void Move()
        {
            int newX = Location.X + Acceleration.X;
            Location = new Point(newX, Location.Y);
        }

        private void SlowDown()
        {
            if (Acceleration.X == 0)
            {
                return;
            }

            if (Acceleration.X > 0)
            {
                Acceleration = new Point(Acceleration.X - 1, 0);
            }
            else if (Acceleration.X < 0)
            {
                Acceleration = new Point(Acceleration.X + 1, 0);
            }
        }

        private void ResolveWindowCollisions(Rectangle window)
        {
            if (window.Width <= Location.X + Size.Width)
            {
                Location = new Point(window.Width - Size.Width, Location.Y);
                Acceleration = new Point(0, 0);
            }
            else if (0 >= Location.X)
            {
                Location = new Point(0, Location.Y);
                Acceleration = new Point(0, 0);
            }
        }

        public void Accelerate(int value)
        {
            if (Acceleration.X == MaxAcceleration && value > 0 ||
                Acceleration.X == -MaxAcceleration && value < 0)
            {
                return;
            }

            int newX = Acceleration.X + value;

            if (newX > MaxAcceleration)
            {
                newX = MaxAcceleration;
            }
            else if (newX < -MaxAcceleration)
            {
                newX = -MaxAcceleration;
            }

            Acceleration = new Point(newX, 0);
        }

        public override void Update(Graphics g, Rectangle window)
        {
            if (Location.X == 0 && Location.Y == 0)
            {
                SetLocation(window);
            }

            Move();
            ResolveWindowCollisions(window);
            base.Update(g, window);
            SlowDown();
        }
    }
}