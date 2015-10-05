using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidGame.GameObjects
{
    public class Ball : GameObjectBase
    {
        public event EventHandler GameOver;

        public Point Acceleration { get; set; }
        public int MaxAcceleration { get; set; }
        public int BottomOffset { get; set; }
        public double Radius => (double)Size.Height/2;

        public Ball(int size, int bottomOffset, int maxAcceleration)
        {
            Size = new Size(size, size);
            Color = Color.OrangeRed;
            Acceleration = new Point(0, 0);
            MaxAcceleration = maxAcceleration;
            BottomOffset = bottomOffset;

            Accelerate(0, maxAcceleration);
        }

        private void SetLocation(Rectangle window)
        {
            Location = new Point(
                window.Width / 2 - Size.Width / 2,
                window.Height - Size.Height - BottomOffset
                );
        }

        private void Move()
        {
            int newX = Location.X + Acceleration.X;
            int newY = Location.Y + Acceleration.Y;
            Location = new Point(newX, newY);
        }

        private void ResolveWindowCollisions(Rectangle window)
        {
            if (window.Height <= Location.Y + Size.Height)
            {
                GameOver?.Invoke(this, new EventArgs());

                Acceleration = new Point(Acceleration.X, -Acceleration.Y);
                Location = new Point(Location.X, window.Height - Size.Height);
            }
            else if (0 >= Location.Y)
            {
                Acceleration = new Point(Acceleration.X, -Acceleration.Y);
                Location = new Point(Location.X, 0);
            }

            if (window.Width <= Location.X + Size.Width)
            {
                Acceleration = new Point(-Acceleration.X, Acceleration.Y);
                Location = new Point(window.Width - Size.Width, Location.Y);
            }
            else if (0 >= Location.X)
            {
                Acceleration = new Point(-Acceleration.X, Acceleration.Y);
                Location = new Point(0, Location.Y);
            }
        }

        public void ResolvePlatformCollisions(Platform platform)
        {
            if (platform.Location.Y <= Location.Y + Size.Height &&
                Location.Y + Size.Height - platform.Location.Y <= Radius &&
                platform.Location.X < Location.X + Size.Width && 
                platform.Location.X + platform.Size.Width > Location.X)
            {
                Acceleration = new Point((int) (Math.Abs(Acceleration.X + platform.Acceleration.X*0.5) > MaxAcceleration
                    ? Math.Sign(Acceleration.X) * MaxAcceleration
                    : Acceleration.X + platform.Acceleration.X*0.5), -Acceleration.Y);
                Location = new Point(Location.X, platform.Location.Y - Size.Height);
            }
        }

        public void ResolveBricksCollisions(IEnumerable<Brick> bricks)
        {
            foreach (Brick brick in bricks)
            {
                bool bottom = brick.Location.Y + brick.Size.Height <= Location.Y + Radius &&
                              brick.Location.Y + brick.Size.Height >= Location.Y  &&
                              brick.Location.X <= Location.X + Size.Width &&
                              brick.Location.X + brick.Size.Width >= Location.X;

                bool top = brick.Location.Y <= Location.Y + Size.Height &&
                           Location.Y + Size.Height - brick.Location.Y <= Radius &&
                           brick.Location.X <= Location.X + Size.Width &&
                           brick.Location.X + brick.Size.Width >= Location.X;

                bool right = brick.Location.Y <= Location.Y + Size.Height &&
                             brick.Location.Y >= Location.Y &&
                             brick.Location.X + brick.Size.Width == Location.X &&
                             brick.Location.X <= Location.X;

                bool left = brick.Location.Y <= Location.Y + Size.Height &&
                            brick.Location.Y + brick.Size.Height >= Location.Y &&
                            brick.Location.X + brick.Size.Width >= Location.X + Size.Width &&
                            brick.Location.X <= Location.X + Size.Width;

                if (bottom)
                {
                    Acceleration = new Point(Acceleration.X, -Acceleration.Y);
                    Location = new Point(Location.X, brick.Location.Y + brick.Size.Height);
                    brick.IsSmashed = true;
                    return;
                }

                if (top)
                {
                    Acceleration = new Point(Acceleration.X, -Acceleration.Y);
                    Location = new Point(Location.X, brick.Location.Y - Size.Height);
                    brick.IsSmashed = true;
                    return;
                }
                /*
                if (left)
                {
                    Acceleration = new Point(-Acceleration.X, Acceleration.Y);
                    Location = new Point(brick.Location.X - Size.Width, Location.Y);
                    brick.IsSmashed = true;
                    return;
                }

                if (right)
                {
                    Acceleration = new Point(-Acceleration.X, Acceleration.Y);
                    Location = new Point(brick.Location.X + brick.Size.Width, Location.Y);
                    brick.IsSmashed = true;
                    return;
                }*/
            }
        }

        public void Accelerate(int value)
        {
            Accelerate(value, value);
        }

        public void Accelerate(int x, int y)
        {
            int newX = Acceleration.X + x > MaxAcceleration ? MaxAcceleration : Acceleration.X + x;
            int newY = Acceleration.Y + y > MaxAcceleration ? MaxAcceleration : Acceleration.Y + y;
            Acceleration = new Point(newX, newY);
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
        }

        protected override void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color), new Rectangle(Location, Size));
        }
    }
}
