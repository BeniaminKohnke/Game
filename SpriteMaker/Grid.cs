namespace SpriteMaker
{
    public partial class Grid : UserControl
    {
        private readonly Pen _blackPen = new(Color.Black, 0.1f);
        private readonly SolidBrush _fillingBrush = new(Color.BlueViolet);
        private readonly Pixel[][] _pixels = new Pixel[2048][];
        public ushort GridWidth { get; set; } = 0;
        public ushort GridHeight { get; set; } = 0;
        public byte Value { get; set; } = 3;

        private static byte x = 64;

        public Grid()
        {
            InitializeComponent();

            Size = new(2048, 2048);

            for(int i = 0; i < 2048; i++)
            {
                _pixels[i] = new Pixel[2048];
                for(int j = 0; j < 2048; j++)
                {
                    _pixels[i][j] = new()
                    {
                        Position = new(i * 10, j * 10),
                    };
                }
            }

            Paint += new PaintEventHandler(Draw);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            MouseClick += new MouseEventHandler(ChangeGrid);
        }

        public void SetGrid(byte[][] grid)
        {

        }

        public byte[][] GetGrid()
        {
            return null;
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
            var pixel = GetPixel();

            if(pixel != null)
            {
                pixel.Value = Value;
                pixel.Color = GetColor(pixel.Value);
            }

            Pixel? GetPixel()
            {
                for(int i = 0; i < GridWidth; i++)
                {
                    for(int j = 0; j < GridHeight; j++)
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
            if(GridWidth > 0 && GridHeight > 0)
            {
                for (int i = 0; i < GridWidth; i++)
                {
                    for (int j = 0; j < GridHeight; j++)
                    {
                        if(_pixels[i][j].IsActive)
                        {
                            args.Graphics.FillRectangle(_pixels[i][j].FillingBrush, _pixels[i][j].Rectangle);
                        }
                    }
                }

                for (int i = 0; i <= GridWidth; i++)
                {
                    args.Graphics.DrawLine(_blackPen, new(i * 10, 0), new(i * 10, GridHeight * 10));
                }

                for (int i = 0; i <= GridHeight; i++)
                {
                    args.Graphics.DrawLine(_blackPen, new(0, i * 10), new(GridWidth * 10, i * 10));
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
