using System.Collections.ObjectModel;

namespace GameAPI
{
    public class Ractangle
    {
        private ReadOnlyCollection<ReadOnlyCollection<byte>>? _grid;
        private (int x, int y) _v1;

        /// <summary>
        ///Position: top-left vertex
        ///<para>This point can be set</para>
        /// </summary>
        public (int x, int y) V1 
        {
            get => _v1; 
            set
            {
                _v1 = value;
                V2 = (_v1.x + _grid?[0]?.Count ?? 0, _v1.y);
                V3 = (_v1.x, _v1.y + _grid?.Count ?? 0);
                V4 = (_v1.x + _grid?[0]?.Count ?? 0, _v1.y + _grid?.Count ?? 0);
            }
        }
        /// <summary>
        ///Position: top-right vertex
        /// </summary>
        public (int x, int y) V2 { get; private set; }
        /// <summary>
        ///Position: bottom-left vertex
        /// </summary>
        public (int x, int y) V3 { get; private set; }
        /// <summary>
        ///Position: bottom-right vertex
        /// </summary>
        public (int x, int y) V4 { get; private set; }

        public int SizeX => _grid?[0]?.Count ?? 0;
        public int SizeY => _grid?.Count ?? 0;
        public byte this[int x, int y] => _grid?[y]?[x] ?? 0;

        public Ractangle(ReadOnlyCollection<ReadOnlyCollection<byte>>? grid, int x, int y)
        {
            _grid = grid;
            _v1 = (x, y);
            V2 = (x + grid?[0]?.Count ?? 0, y);
            V3 = (x, y + grid?.Count ?? 0);
            V4 = (x + grid?[0]?.Count ?? 0, y + grid?.Count ?? 0);
        }

        public void SetGrid(ReadOnlyCollection<ReadOnlyCollection<byte>>? grid)
        {
            _grid = grid;
            _v1 = (_v1.x, _v1.y);
            V2 = (_v1.x + _grid?[0]?.Count ?? 0, _v1.y);
            V3 = (_v1.x, _v1.y + _grid?.Count ?? 0);
            V4 = (_v1.x + _grid?[0]?.Count ?? 0, _v1.y + _grid?.Count ?? 0);
        }

        public Ractangle Copy(int? x = 0, int? y = 0) => new(_grid, x ?? V1.x, y ?? V2.y);
        public Ractangle CopyWithShift(int x = 0, int y = 0) => new(_grid, V1.x + x, V2.y + y);

        public bool CheckCollision(Ractangle other)
        {
            if(_grid != null && other.SizeX != 0)
            {
                var v1 = (x: V1.x < other.V1.x ? V1.x : other.V1.x, y: V1.y < other.V1.y ? V1.y : other.V1.y);
                var v4 = (x: V4.x > other.V4.x ? V4.x : other.V4.x, y: V4.y > other.V4.y ? V4.y : other.V4.y);

                var sizeX = SizeX + other.SizeX;
                var sizeY = SizeY + other.SizeY;
                if (Math.Abs(v1.x - v4.x) - 1 <= sizeX || Math.Abs(v1.y - v4.y) - 1 <= sizeY)
                {
                    var v2 = (x: V2.x > other.V2.x ? V2.x : other.V2.x, y: V2.y > other.V2.y ? V2.y : other.V2.y);
                    //var v3 = (x: V3.x < other.V3.x ? V3.x : other.V3.x, y: V3.y < other.V3.y ? V3.y : other.V3.y);

                    var grid = new Dictionary<(int, int), byte>();
                    for(int i = -sizeX; i <= sizeX * 2; i++)
                    {
                        for(int j = -sizeY; j <= sizeY * 2; j++)
                        {
                            grid[(v1.x + i, v1.y + j)] = 0;
                        }
                    }

                    for(int i = 0; i < SizeX; i++)
                    {
                        for(int j = 0; j < SizeY; j++)
                        {
                            var value = this[i, j];
                            if(value == 3 || value == 5 || value == 6)
                            {
                                grid[(i + V1.x, j + V1.y)]++;
                            }
                        }
                    }

                    for (int i = 0; i < other.SizeX; i++)
                    {
                        for (int j = 0; j < other.SizeY; j++)
                        {
                            var value = other[i, j];
                            if (value == 3 || value == 5 || value == 6)
                            {
                                grid[(i + other.V1.x, j + other.V1.y)]++;
                            }
                        }
                    }

                    return grid.Values.Any(v => v == 2);
                }
            }

            return false;
        }
    }
}
