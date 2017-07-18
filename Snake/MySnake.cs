using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Snake
{
    class MySnake
    {    
        public SnakePart Glowa { get; private set; }
        public List<SnakePart> Czesci { get; private set; }
    
    public MySnake() //głowa
    {
            Glowa = new SnakePart(20, 0);
            Glowa.Kwadrat.Width = Glowa.Kwadrat.Height = 10;
            Glowa.Kwadrat.Fill = Brushes.Red;
            Czesci = new List<SnakePart>();
            //reszta weza
            Czesci.Add(new SnakePart(19, 0));
            Czesci.Add(new SnakePart(18, 0));
            Czesci.Add(new SnakePart(17, 0));
            Czesci.Add(new SnakePart(16, 0));
            Czesci.Add(new SnakePart(15, 0));
            Czesci.Add(new SnakePart(14, 0));
            Czesci.Add(new SnakePart(13, 0));
            Czesci.Add(new SnakePart(12, 0));
            Czesci.Add(new SnakePart(11, 0));
            Czesci.Add(new SnakePart(10, 0));
        }
    public void RedrawSnake()   //rysuj
        {
            Grid.SetColumn(Glowa.Kwadrat, Glowa.x);
            Grid.SetRow(Glowa.Kwadrat, Glowa.y);
            foreach (SnakePart snakePart in Czesci)
            {
                Grid.SetColumn(snakePart.Kwadrat, snakePart.x);
                Grid.SetRow(snakePart.Kwadrat, snakePart.y);
            }
        }

    }
}
