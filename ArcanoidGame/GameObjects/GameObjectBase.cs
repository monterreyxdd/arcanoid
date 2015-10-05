using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcanoidGame.GameObjects
{
    public class GameObjectBase
    {
        public Point Location { get; protected set; }
        public Size Size { get; protected set; }
        public Color Color { get; protected set; }

        public virtual void Update(Graphics g, Rectangle window)
        {
            Draw(g);
        }

        protected virtual void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color), new Rectangle(Location, Size));
        }
    }
}
