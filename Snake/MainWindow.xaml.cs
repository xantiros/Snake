using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MySnake waz;
        private static readonly int SIZE = 9;               //rozmiar całości

        private int _directionX = 1;
        private int _directionY = 0;
        private DispatcherTimer _timer;

        private SnakePart _food;
        private int _partsToAdd;

        private List<Wall> _walls;

        public MainWindow() //moja zmiana
        {
            InitializeComponent();
            InitBoard();
            InitSnake();
            InitTimer();
            InitFood();
            InitWall();
        }
        void InitBoard()    //inicjacja planszy
        {
            for (int i = 0; i < grid.Width/SIZE; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(SIZE);
                grid.ColumnDefinitions.Add(columnDefinition);
            }
            for (int i = 0; i < grid.Height/SIZE; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(SIZE);
                grid.RowDefinitions.Add(rowDefinition);
            }
            waz = new MySnake();
        }
        void InitSnake()
        {
            grid.Children.Add(waz.Glowa.Kwadrat);
            foreach (SnakePart snakePart in waz.Czesci)
            {
                grid.Children.Add(snakePart.Kwadrat);
                waz.RedrawSnake();
            }
        }
        private void MoveSnake()
        {
            int snakePartCount = waz.Czesci.Count;
            if (_partsToAdd > 0)
            {
                SnakePart newPart = new SnakePart(waz.Czesci[waz.Czesci.Count - 1].x,
                    waz.Czesci[waz.Czesci.Count - 1].y);
                grid.Children.Add(newPart.Kwadrat);
                waz.Czesci.Add(newPart);
                _partsToAdd--;
            }
            for (int i = waz.Czesci.Count - 1; i >= 1; i--)
            {
                waz.Czesci[i].x = waz.Czesci[i - 1].x;
                waz.Czesci[i].y = waz.Czesci[i - 1].y;
            }
            waz.Czesci[0].x = waz.Glowa.x;
            waz.Czesci[0].y = waz.Glowa.y;
            waz.Glowa.x += _directionX;
            waz.Glowa.y += _directionY;
            
            if (CheckCollision())
            {
                EndGame();
            }
            else
            {
                if (CheckFood())
                    RedrawFood();
                waz.RedrawSnake();
                waz.RedrawSnake();
            }
            
        }
        void InitTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(_timer_tick);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 40);
            _timer.Start();
        }
        void _timer_tick(object sender, EventArgs e)
        {
            MoveSnake();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left)
            {
                _directionX = -1;
                _directionY = 0;
            }
            if (e.Key == Key.Right)
            {
                _directionX = 1;
                _directionY = 0;
            }
            if (e.Key == Key.Up)
            {
                _directionX = 0;
                _directionY = -1;
            }
            if (e.Key == Key.Down)
            {
                _directionX = 0;
                _directionY = 1;
            }
        }
        
        void InitFood()
        {
            Random rand = new Random();
            _food = new SnakePart(rand.Next(0,(int)(grid.Width/SIZE)), rand.Next(0, (int)(grid.Height / SIZE)));
            _food.Kwadrat.Width = _food.Kwadrat.Height = 10;
            _food.Kwadrat.Fill = Brushes.Blue;
            grid.Children.Add(_food.Kwadrat);
            Grid.SetColumn(_food.Kwadrat, _food.x);
            Grid.SetRow(_food.Kwadrat, _food.y);
        }
        private bool CheckFood()
        {
            Random rand = new Random();
            if (waz.Glowa.x == _food.x && waz.Glowa.y == _food.y)
            {
                _partsToAdd += 20;                               //ilosc jedzeniea dodawanego
                for (int i = 0; i < 20; i++)
                {
                    int x = rand.Next(0, (int)(grid.Width / SIZE));
                    int y = rand.Next(0, (int)(grid.Height / SIZE));
                    if (IsFieldFree(x,y))
                    {
                        _food.x = x;
                        _food.y = y;
                        return true;
                    }
                }
                for (int i = 0; i < grid.Width/SIZE; i++)
                {
                    for (int j = 0; j < grid.Height/SIZE; j++)
                    {
                        if(IsFieldFree(i,j))
                        {
                            _food.x = i;
                            _food.y = j;
                            return true;
                        }
                    }
                    
                }
                EndGame();
            }
            return false;
        }

        private bool IsFieldFree(int x, int y)
        {
            if (waz.Glowa.x == x && waz.Glowa.y == y) return false;
            foreach (SnakePart snakePart in waz.Czesci)
            {
                if (snakePart.x == x && snakePart.y == y) return false;
            }
            return true;    
        }

        void EndGame()
        {
            _timer.Stop();
            MessageBox.Show("Koniec Gry");
            //System.Diagnostics.Process.Start("Snake.exe");
        }
        private void RedrawFood()
        {
            Grid.SetColumn(_food.Kwadrat, _food.x);
            Grid.SetRow(_food.Kwadrat, _food.y);
        }

        bool CheckCollision()
        {
            if (CheckBoardCollision())
                return true;
            if (CheckItselfCollision())
                return true;
            if (CheckWallCollision())
                return true;
            return false;
        }

        bool CheckBoardCollision()              //kolizja waz - koniec planszy
        {
            if (waz.Glowa.x < 0 || waz.Glowa.x > grid.Width / SIZE)
                return true;
            if (waz.Glowa.y < 0 || waz.Glowa.y > grid.Height / SIZE)
                return true;
            return false;
        }

        bool CheckItselfCollision()             //kolizja waz - waz
        {
            foreach (SnakePart snakePart in waz.Czesci)
            {
                if (waz.Glowa.x == snakePart.x && waz.Glowa.y == snakePart.y)
                    return true;
            }
            return false;
        }

        void InitWall()                 //przeszkody
        {
            _walls = new List<Wall>();
            Wall wall1 = new Wall(15, 10, 10, 2);
            grid.Children.Add(wall1.Rect);
            Grid.SetColumn(wall1.Rect, wall1.X);
            Grid.SetRow(wall1.Rect, wall1.Y);
            Grid.SetColumnSpan(wall1.Rect, wall1.Width);
            Grid.SetRowSpan(wall1.Rect, wall1.Height);
            _walls.Add(wall1);

            //Wall wall2 = new Wall(39, 15, 3, 30);
            //grid.Children.Add(wall2.Rect);
            //Grid.SetColumn(wall2.Rect, wall2.X);
            //Grid.SetRow(wall2.Rect, wall2.Y);
            //Grid.SetColumnSpan(wall2.Rect, wall2.Width);
            //Grid.SetRowSpan(wall2.Rect, wall2.Height);
            //_walls.Add(wall2);

            //Wall wall3 = new Wall(59, 15, 3, 30);
            //grid.Children.Add(wall3.Rect);
            //Grid.SetColumn(wall3.Rect, wall3.X);
            //Grid.SetRow(wall3.Rect, wall3.Y);
            //Grid.SetColumnSpan(wall3.Rect, wall3.Width);
            //Grid.SetRowSpan(wall3.Rect, wall3.Height);
            //_walls.Add(wall3);
        }

        bool CheckWallCollision()       //kolizja ze sciana
        {
            foreach (Wall wall in _walls)
            {
                if (waz.Glowa.x >= wall.X && waz.Glowa.x < wall.X + wall.Width &&
                waz.Glowa.y >= wall.Y && waz.Glowa.y < wall.Y + wall.Height)
                    return true;
            }
            return false;
        }

    }
}
