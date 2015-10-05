using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidGame.GameObjects
{
    public class Brick : GameObjectBase
    {
        public bool IsSmashed { get; set; }

        public Brick(int x, int y, int width, int height)
        {
            Size = new Size(width, height);
            Color = Color.DarkOliveGreen;
            Location = new Point(x, y);
        }

        protected override void Draw(Graphics g)
        {
            g.FillRectangle(new LinearGradientBrush(Location, new Point(Location.X + Size.Width, Location.Y + Size.Height), 
                Color.BlueViolet, Color.Gold), Location.X, Location.Y, Size.Width, Size.Height);
        }
    }
}
