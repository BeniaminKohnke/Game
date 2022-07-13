namespace SpriteMaker
{
    public partial class Grid : UserControl
    {
        private const ushort GRID_SIZE = 2048;
        private ushort _width = 0;
        private ushort _height = 0;
        private readonly Pen _blackPen = new(Color.Black, 0.1f);
        private readonly Pixel[][] _pixels = new Pixel[GRID_SIZE][];
        public byte Value { get; set; } = 4;
        public ushort GridWidth
        {
            get => _width;
            set
            {
                _width = value;
                ResizeGrid();
            }
        }
        public ushort GridHeight
        {
            get => _height;
            set
            {
                _height = value;
                ResizeGrid();
            }
        }

        private void ResizeGrid()
        {
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for(int j = 0; j < GRID_SIZE; j++)
                {
                    if(i < _height && j < _width)
                    {
                        if (_pixels[i][j].Value == 0)
                        {
                            _pixels[i][j].Value = 7;
                            _pixels[i][j].IsActive = true;
                            _pixels[i][j].Color = Color.BlueViolet;
                        }
                    }
                    else
                    {
                        _pixels[i][j].Value = 0;
                        _pixels[i][j].IsActive = false;
                        _pixels[i][j].Color = Color.BlueViolet;
                    }
                }
            }
        }

        public Grid()
        {
            InitializeComponent();

            Size = new(2048, 2048);

            for(int i = 0; i < GRID_SIZE; i++)
            {
                _pixels[i] = new Pixel[GRID_SIZE];
                for(int j = 0; j < GRID_SIZE; j++)
                {
                    _pixels[i][j] = new()
                    {
                        Position = new(j * 10, i * 10),
                    };
                }
            }

            Paint += new PaintEventHandler(Draw);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            MouseMove += new MouseEventHandler(ChangeGrid);
            MouseClick += new MouseEventHandler(ChangeGrid);
        }

        public void SetGrid(byte[][] grid)
        {
            _height = (ushort)grid.Length;
            _width = (ushort)grid[0].Length;
            ResizeGrid();

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    _pixels[i][j].Value = grid[i][j];
                    _pixels[i][j].Color = GetColor(grid[i][j]);
                }
            }
        }

        public byte[][] GetGrid()
        {
            var grid = new byte[_height][].Select(l => l = new byte[_width]).ToArray();
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    grid[i][j] = _pixels[i][j].Value;
                }
            }

            return grid;
        }

        private static Color GetColor(byte value) => value switch
        {
            5 => Color.Gray,
            3 => Color.Yellow,
            4 => Color.White,
            2 => Color.LightSteelBlue,
            6 => Color.Fuchsia,
            _ => Color.BlueViolet,
        };

        private void ChangeGrid(object? sender, MouseEventArgs args)
        {
            if(args.Button == MouseButtons.Left)
            {
                var pixel = GetPixel();
                if(pixel != null)
                {
                    pixel.Value = Value;
                    pixel.Color = GetColor(pixel.Value);
                }
            }

            if(args.Button == MouseButtons.Right)
            {
                var pixel = GetPixel();
                if (pixel != null)
                {
                    pixel.Value = 7;
                    pixel.Color = Color.BlueViolet;
                }
            }

            Pixel? GetPixel()
            {
                for(int i = 0; i < _height; i++)
                {
                    for(int j = 0; j < _width; j++)
                    {
                        if(_pixels[i][j].Position.X <= args.X
                            && args.X < _pixels[i][j].Position.X + _pixels[i][j].Size.Width
                            && _pixels[i][j].Position.Y - _pixels[i][j].Size.Height < args.Y
                            && args.Y <= _pixels[i][j].Position.Y + 5)
                        {
                            return _pixels[i][j];
                        }
                    }
                }
                return null;
            }
        }

        private void Draw(object? sender, PaintEventArgs args)
        {
            args.Graphics.Clear(Color.BlueViolet);
            if(_width > 0 && _height > 0)
            {
                for (int i = 0; i < _height; i++)
                {
                    for (int j = 0; j < _width; j++)
                    {
                        if(_pixels[i][j].IsActive)
                        {
                            args.Graphics.FillRectangle(_pixels[i][j].FillingBrush, _pixels[i][j].Rectangle);
                        }
                    }
                }

                for (int i = 0; i <= _width; i++)
                {
                    args.Graphics.DrawLine(_blackPen, new(i * 10, 0), new(i * 10, _height * 10));
                }

                for (int i = 0; i <= _height; i++)
                {
                    args.Graphics.DrawLine(_blackPen, new(0, i * 10), new(_width * 10, i * 10));
                }
            }

            Invalidate();
        }

        private record Pixel
        {
            private Size _size = new(10, 10);
            private Point _position = new(0, 0);
            public SolidBrush FillingBrush { get; } = new(Color.BlueViolet);
            public Rectangle Rectangle { get; private set; }
            public bool IsActive { get; set; } = true;
            public byte Value { get; set; } = 0;
            public Size Size 
            { 
                get => _size; 
                set
                {
                    _size = value;
                    Rectangle = new Rectangle(_position, _size);
                }
            }
            public Point Position
            {
                get => _position;
                set
                {
                    _position = value;
                    Rectangle = new Rectangle(_position, _size);
                }
            }
            public Color Color
            {
                get => FillingBrush.Color;
                set => FillingBrush.Color = value;
            }
        }
    }
}
