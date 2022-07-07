using System.Collections.ObjectModel;

namespace GameAPI
{
    public class Rectangle
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

        public Rectangle(ReadOnlyCollection<ReadOnlyCollection<byte>>? grid, int x, int y)
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

        public Rectangle Copy(int? x = 0, int? y = 0) => new(_grid, x ?? V1.x, y ?? V2.y);
        public Rectangle CopyWithShift(int x = 0, int y = 0) => new(_grid, V1.x + x, V2.y + y);

        public bool CheckCollision(Rectangle other)
        {
            if(_grid != null && other.SizeX != 0)
            {
                var x1 = V1.x < other.V1.x ? V1.x : other.V1.x;
                var x2 = V4.x > other.V4.x ? V4.x : other.V4.x;
                var y1 = V1.y < other.V1.y ? V1.y : other.V1.y;
                var y2 = V4.y > other.V4.y ? V4.y : other.V4.y;

                if ((x2 - x1 - 1 <= SizeX + other.SizeX) && (y2 - y1 - 1 <= SizeY + other.SizeY))
                {
                    var positionDict = new Dictionary<int, int>();
                    for(int i = 0; i < SizeX; i++)
                    {
                        for(int j = 0; j < SizeY; j++)
                        {
                            if(6 % this[i, j] == 0)
                            {
                                positionDict[V1.x + i] = V1.y + j;
                            }
                        }
                    }

                    for (int i = 0; i < other.SizeX; i++)
                    {
                        for (int j = 0; j < other.SizeY; j++)
                        {
                            if (6 % other[i, j] == 0)
                            {
                                var position = other.V1.x + i;
                                if (positionDict.ContainsKey(position) && positionDict[position] == other.V1.y + j)
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
