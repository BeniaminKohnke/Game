using System.Collections.ObjectModel;

namespace GameAPI
{
    public class Rectangle
    {
        private ReadOnlyCollection<ReadOnlyCollection<byte>>? _grid;
        private (int x, int y) _topLeft;
        private (int x, int y) _bottomRight;

        public (int x, int y) Position 
        {
            get => _topLeft; 
            set
            {
                _topLeft = value;
                _bottomRight = (_topLeft.x + _grid?[0]?.Count ?? 0, _topLeft.y + _grid?.Count ?? 0);
            }
        }

        public int SizeX => _grid?[0]?.Count ?? 0;
        public int SizeY => _grid?.Count ?? 0;
        public byte this[int x, int y] => _grid?[y]?[x] ?? 0;

        public Rectangle(ReadOnlyCollection<ReadOnlyCollection<byte>>? grid, int x, int y)
        {
            _grid = grid;
            _topLeft = (x, y);
            _bottomRight = (x + grid?[0]?.Count ?? 0, y + grid?.Count ?? 0);
        }

        public void SetGrid(ReadOnlyCollection<ReadOnlyCollection<byte>>? grid)
        {
            _grid = grid;
            _topLeft = (_topLeft.x, _topLeft.y);
            _bottomRight = (_topLeft.x + _grid?[0]?.Count ?? 0, _topLeft.y + _grid?.Count ?? 0);
        }

        public Rectangle Copy(int? x = 0, int? y = 0) => new(_grid, x ?? Position.x, y ?? Position.y);
        public Rectangle CopyWithShift(int x = 0, int y = 0) => new(_grid, Position.x + x, Position.y + y);

        public bool CheckCollision(Rectangle other)
        {
            if(_grid != null && other.SizeX != 0)
            {
                if ((Math.Max(_bottomRight.x, other._bottomRight.x) - Math.Min(_topLeft.x, other._topLeft.x) - 1 <= SizeX + other.SizeX) 
                    && (Math.Max(_bottomRight.y, other._bottomRight.y) - Math.Min(_topLeft.y, other._topLeft.y) - 1 <= SizeY + other.SizeY))
                {
                    var positions = new Dictionary<int, int>();
                    for(int i = 0; i < SizeX; i++)
                    {
                        for(int j = 0; j < SizeY; j++)
                        {
                            if(6 % this[i, j] == 0)
                            {
                                positions[_topLeft.x + i] = _topLeft.y + j;
                            }
                        }
                    }

                    for (int i = 0; i < other.SizeX; i++)
                    {
                        for (int j = 0; j < other.SizeY; j++)
                        {
                            if (6 % other[i, j] == 0)
                            {
                                var position = other._topLeft.x + i;
                                if (positions.ContainsKey(position) && positions[position] == other._topLeft.y + j)
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
