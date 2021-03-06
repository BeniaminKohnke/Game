namespace SpriteMaker
{
    public partial class Grid : UserControl
    {
        private const ushort GRID_SIZE = 2048;
        private byte _pixelSize = 10;
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
        public byte PixelSize
        {
            get => _pixelSize;
            set
            {
                _pixelSize = value;
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    for (int j = 0; j < GRID_SIZE; j++)
                    {
                        _pixels[i][j].Size = _pixelSize;
                        _pixels[i][j].Position = new(j * _pixelSize, i * _pixelSize);
                    }
                }
            }
        }

        public Grid()
        {
            InitializeComponent();

            Size = new(GRID_SIZE, GRID_SIZE);

            for(int i = 0; i < GRID_SIZE; i++)
            {
                _pixels[i] = new Pixel[GRID_SIZE];
                for(int j = 0; j < GRID_SIZE; j++)
                {
                    _pixels[i][j] = new()
                    {
                        Size = _pixelSize,
                        Position = new(j * _pixelSize, i * _pixelSize),
                    };
                }
            }

            Paint += new(Draw);
            MouseMove += new(ChangeGrid);
            MouseClick += new(ChangeGrid);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
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
                        }
                    }
                    else
                    {
                        _pixels[i][j].Value = 0;
                    }
                }
            }
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

        private void ChangeGrid(object? sender, MouseEventArgs args)
        {
            if(args.Button == MouseButtons.Left)
            {
                var pixel = GetPixel();
                if(pixel != null)
                {
                    pixel.Value = Value;
                }
            }

            if(args.Button == MouseButtons.Right)
            {
                var pixel = GetPixel();
                if (pixel != null)
                {
                    pixel.Value = 7;
                }
            }

            if(args.Button == MouseButtons.Middle)
            {
                var pixel = GetPixel();
                if (pixel != null)
                {
                    var j = _pixels.Select(a => Array.IndexOf(a, pixel)).FirstOrDefault(v => v > 0);
                    var i = Array.IndexOf(_pixels, _pixels.Where(a => Array.IndexOf(a, pixel) > 0).FirstOrDefault());

                    var value = pixel.Value;
                    if(value != Value)
                    {
                        var positionsToCheck = new Queue<(int i, int j)>();
                        positionsToCheck.Enqueue((i, j));
                        while (positionsToCheck.TryDequeue(out var position))
                        {
                            if (_pixels[position.i][position.j].Value == value)
                            {
                                _pixels[position.i][position.j].Value = Value;
                                if (position.i > 0)
                                {
                                    var positionToCheck = (i: position.i - 1, position.j);
                                    if (_pixels[positionToCheck.i][positionToCheck.j].Value == value)
                                    {
                                        positionsToCheck.Enqueue(positionToCheck);
                                    }
                                }
                                if (position.j > 0)
                                {
                                    var positionToCheck = (position.i, j: position.j - 1);
                                    if (_pixels[positionToCheck.i][positionToCheck.j].Value == value)
                                    {
                                        positionsToCheck.Enqueue(positionToCheck);
                                    }
                                }
                                if (position.i < _height - 1)
                                {
                                    var positionToCheck = (i: position.i + 1, position.j);
                                    if (_pixels[positionToCheck.i][positionToCheck.j].Value == value)
                                    {
                                        positionsToCheck.Enqueue(positionToCheck);
                                    }
                                }
                                if (position.j < _width - 1)
                                {
                                    var positionToCheck = (position.i, j: position.j + 1);
                                    if (_pixels[positionToCheck.i][positionToCheck.j].Value == value)
                                    {
                                        positionsToCheck.Enqueue(positionToCheck);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Pixel? GetPixel()
            {
                for(int i = 0; i < _height; i++)
                {
                    for(int j = 0; j < _width; j++)
                    {
                        if(_pixels[i][j].Position.X <= args.X
                            && args.X < _pixels[i][j].Position.X + _pixels[i][j].Size
                            && _pixels[i][j].Position.Y - _pixels[i][j].Size < args.Y
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
                    args.Graphics.DrawLine(_blackPen, new(i * _pixelSize, 0), new(i * _pixelSize, _height * _pixelSize));
                }

                for (int i = 0; i <= _height; i++)
                {
                    args.Graphics.DrawLine(_blackPen, new(0, i * _pixelSize), new(_width * _pixelSize, i * _pixelSize));
                }
            }

            Invalidate();
        }

        private record Pixel
        {
            private byte _value = 0;
            private byte _size;
            private Point _position = new(0, 0);
            public SolidBrush FillingBrush { get; } = new(Color.BlueViolet);
            public Rectangle Rectangle { get; private set; }
            public bool IsActive { get; private set; } = true;
            public byte Value
            {
                get => _value;
                set
                {
                    _value = value;
                    IsActive = _value != 0;
                    FillingBrush.Color = _value switch
                    {
                        5 => Color.Gray,
                        3 => Color.Yellow,
                        4 => Color.White,
                        2 => Color.LightSteelBlue,
                        6 => Color.Fuchsia,
                        _ => Color.BlueViolet
                    };
                }
            }
            public byte Size 
            { 
                get => _size; 
                set
                {
                    _size = value;
                    Rectangle = new(_position, new(_size, _size));
                }
            }
            public Point Position
            {
                get => _position;
                set
                {
                    _position = value;
                    Rectangle = new(_position, new(_size, _size));
                }
            }
        }
    }
}
