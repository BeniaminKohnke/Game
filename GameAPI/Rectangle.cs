using System.Collections.ObjectModel;

namespace GameAPI
{
    public class Rectangle
    {
        private ReadOnlyCollection<ReadOnlyCollection<byte>>? _grid;
        private (int x, int y) _v1;
        private (int x, int y) _v2;

        /// <summary>
        ///Position: top-left vertex
        ///<para>This point can be set</para>
        /// </summary>
        public (int x, int y) Position 
        {
            get => _v1; 
            set
            {
                _v1 = value;
                _v2 = (_v1.x + _grid?[0]?.Count ?? 0, _v1.y + _grid?.Count ?? 0);
            }
        }

        public int SizeX => _grid?[0]?.Count ?? 0;
        public int SizeY => _grid?.Count ?? 0;
        public byte this[int x, int y] => _grid?[y]?[x] ?? 0;

        public Rectangle(ReadOnlyCollection<ReadOnlyCollection<byte>>? grid, int x, int y)
        {
            _grid = grid;
            _v1 = (x, y);
            _v2 = (x + grid?[0]?.Count ?? 0, y + grid?.Count ?? 0);
        }

        public void SetGrid(ReadOnlyCollection<ReadOnlyCollection<byte>>? grid)
        {
            _grid = grid;
            _v1 = (_v1.x, _v1.y);
            _v2 = (_v1.x + _grid?[0]?.Count ?? 0, _v1.y + _grid?.Count ?? 0);
        }

        public Rectangle Copy(int? x = 0, int? y = 0) => new(_grid, x ?? Position.x, y ?? Position.y);
        public Rectangle CopyWithShift(int x = 0, int y = 0) => new(_grid, Position.x + x, Position.y + y);

        public bool CheckCollision(Rectangle other)
        {
            if(_grid != null && other.SizeX != 0)
            {
                if ((Math.Max(_v2.x, other._v2.x) - Math.Min(Position.x, other.Position.x) - 1 <= SizeX + other.SizeX) 
                    && (Math.Max(_v2.y, other._v2.y) - Math.Min(Position.y, other.Position.y) - 1 <= SizeY + other.SizeY))
                {
                    var positions = new Dictionary<int, int>();
                    for(int i = 0; i < SizeX; i++)
                    {
                        for(int j = 0; j < SizeY; j++)
                        {
                            if(6 % this[i, j] == 0)
                            {
                                positions[Position.x + i] = Position.y + j;
                            }
                        }
                    }

                    for (int i = 0; i < other.SizeX; i++)
                    {
                        for (int j = 0; j < other.SizeY; j++)
                        {
                            if (6 % other[i, j] == 0)
                            {
                                var position = other.Position.x + i;
                                if (positions.ContainsKey(position) && positions[position] == other.Position.y + j)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
