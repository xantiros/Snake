using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Snake
{
    class SnakePart //czesci weza 
    {
        public int x { get; set; }
        public int y { get; set; }
        public Rectangle Kwadrat { get; private set; }

        public SnakePart(int x, int y)
        {
            this.x = x;
            this.y = y;
            Kwadrat = new Rectangle();
            Kwadrat.Width = Kwadrat.Height = 9;
            Kwadrat.Fill = Brushes.Black;
        }
    }
}
